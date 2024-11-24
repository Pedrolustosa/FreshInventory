import { ApplicationConfig, importProvidersFrom } from '@angular/core';
import { provideRouter } from '@angular/router';
import { routes } from './app.routes';
import { BrowserAnimationsModule, provideAnimations } from '@angular/platform-browser/animations';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { provideToastr, ToastrModule } from 'ngx-toastr';
import { NgxSpinnerModule } from 'ngx-spinner';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { jwtInterceptor } from './interceptors/jwt.interceptor';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes),
    provideAnimations(),
    provideRouter([]),
    provideHttpClient(withInterceptors([jwtInterceptor])),
    provideToastr({
      timeOut: 3000,
      positionClass: 'toast-top-right',
      preventDuplicates: true,
      progressBar: true,
      closeButton: true,
      enableHtml: true,
      toastClass: 'toast ngx-toastr',
      titleClass: 'toast-title',
      messageClass: 'toast-message',
      iconClasses: {
        error: 'toast-error',
        info: 'toast-info',
        success: 'toast-success',
        warning: 'toast-warning'
      }
    }),
    importProvidersFrom(
      BsDatepickerModule.forRoot(),
      BsDropdownModule.forRoot(),
      BrowserAnimationsModule,
      ToastrModule.forRoot(),
      NgxSpinnerModule.forRoot()
    )
  ]
};