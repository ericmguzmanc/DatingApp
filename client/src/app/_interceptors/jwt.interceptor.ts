import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { AccountService } from '../_services/account.service';
import { User } from '../_models/user';
import { take } from 'rxjs/operators';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {

  constructor(private accountService: AccountService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    let currentUser: User;
    // ❗️ Important, by taking just one, we dont need to unsubscribe since it only takes the first observable
    // Whenever we are not sure if we need to unsubscribe, we could use this.
    this.accountService.currentUser$.pipe(take(1)).subscribe(user => currentUser = user);
    if (currentUser) {
      // ℹ️ Clone request and add authentication header 
      request = request.clone({
        setHeaders: {
          Authorization: `Bearer ${currentUser.token}` 
        }
      });
    }

    return next.handle(request);
  }
}
