import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgxSpinnerModule } from 'ngx-spinner';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';

import { jwtInterceptor } from './interceptors/jwt.interceptor';
import { ToastrModule } from 'ngx-toastr';

@NgModule({
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    NgxSpinnerModule.forRoot(),
    BsDropdownModule.forRoot(),
    ToastrModule.forRoot(),
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useValue: jwtInterceptor, multi: true }
  ]
})
export class AppModule { }