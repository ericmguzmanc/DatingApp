import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { NavigationExtras, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { catchError } from 'rxjs/operators';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

  constructor(private router: Router, private toastr: ToastrService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      catchError(error => {
        if (error) {
          switch (error.status) {
            case 400:
              if (error.error.errors) {
                // ℹ️ known on .net - modalStateErros are validation erros
                let modalStateErros = [];
                for (const key in error.error.errors) {
                  if (error.error.errors[key]) {
                    modalStateErros.push(error.error.errors[key]);
                  }
                }
                // ℹ️ This is going to be usefull when returning validation errors
                // Lets say in a form like the register
                throw modalStateErros.flat();
              } else if (typeof(error.error) == 'object') {
                this.toastr.error(error.statusText, error.status);
              } else {
                this.toastr.error(error.error, error.status);
              }
              break;
              case 401:
                this.toastr.error(error.statusText, error.status);
                break;
              case 404:
                this.router.navigateByUrl('/not-found');
                break;
              case 500:
                // 🔎 Set navigation extras to the server-error page, usefull to display
                // the error on that page
                const navigationExtras: NavigationExtras = {state: {error: error.error}};
                this.router.navigateByUrl('/server-error', navigationExtras);
                break;
            default:
              this.toastr.error('Something unexpected went wrong');
              console.log(error);
              break;
          }
        }
        return throwError(error);
      })
    );
  }
}
