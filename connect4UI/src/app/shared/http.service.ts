import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders,  HttpErrorResponse } from '@angular/common/http';
import { Observable,throwError } from 'rxjs';
import { map, catchError, retry } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
/** 
Core Service of the Application which enables Http Interaction with Server ApI
*/
const httpOptionsUrlEncoded = {
  headers: new HttpHeaders({ 
    'Content-Type': 'application/x-www-form-urlencoded', 
    'Authorization': 'auth-token'
})
};

const httpOptions = {
  headers: new HttpHeaders({
    'Content-Type':  'application/json',
    'Authorization': 'auth-token'
  })
};

@Injectable({
  providedIn: 'root'
})
export class HttpService {

  private baseUrl =environment.webApiUrl;
  constructor(
    private httpClient: HttpClient
    ) { }

      /**
     * Constructs a `POST` request that interprets the body as a
     * JSON object and returns the response body as a JSON object.
     *
     * @param endpoint The endpoint URL.
     * @param body The content to replace with.
     *  
     *
     * @return An `Observable` of the response, with the response body as a JSON object.
  */
    
    public Execute(endpoint:string,body: any): Observable<any> {
      console.log('Request method called');      
      return this.httpClient
        .post(`${this.baseUrl}/${endpoint}`, body,httpOptions)
        .pipe(catchError(this.handleError))       
        .pipe(map((response: any) => response)
        );
    }   
    public ExecuteWithURLEncoded(endpoint:string,body: any): Observable<any> {
      
      console.log('Request method called');      
      return this.httpClient
        .post(`${this.baseUrl}/${endpoint}`, body,httpOptionsUrlEncoded)
        .pipe(catchError(this.handleError))       
        .pipe(map((response: any) => response)
        );
    }
  
  
    Request(endpoint:string,retryOption:number=0): Observable<any> {
      //add spinner or loading message for user    
      return this.httpClient
        .get(`${this.baseUrl}/${endpoint}`,httpOptions)
        .pipe(retry(retryOption))
        .pipe(catchError(this.handleError));  
        
    }

   

    //handle error on the service
    private handleError(error: HttpErrorResponse) {
      let errorMessage = '';
      console.log('hanldeError');
      if (error.error instanceof ErrorEvent) {
        // A client-side or network error occurred. Handle it accordingly.
        errorMessage=`Error: ${error.error.message}`;
        console.error('An error occurred:', error.error.message);
      } else {
        // The backend returned an unsuccessful response code.
        // The response body may contain clues as to what went wrong,
        errorMessage=`Error Code: ${error.status}\nMessage: ${error.message}`;
        console.error(`Backend returned code ${error.status}, ` +
          `body was: ${error.error}`);
      }
      // return an observable with a user-facing error message
      //window.alert(errorMessage);
       return throwError(error);
    };


}
