import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { Ka4Component } from './ka4.component';

describe('Ka4Component', () => {
  let component: Ka4Component;
  let fixture: ComponentFixture<Ka4Component>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ Ka4Component ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Ka4Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
