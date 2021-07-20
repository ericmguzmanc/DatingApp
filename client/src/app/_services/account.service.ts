import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { map } from 'rxjs/operators';
import { User } from '../_models/user';
import { ReplaySubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl = environment.apiUrl;
  // ℹ️ Any time is subscribed, it's going to emmit the last value or any quantity we want
  private currentUserSource = new ReplaySubject<User>(1);
  // ℹ️ $ sign at the end used for observable conventions
  // We set the currentUser$ = to the currentUserSource as observable
  currentUser$ = this.currentUserSource.asObservable();

  constructor(private http: HttpClient) { }

  login(model: any) {
    return this.http.post<User>(this.baseUrl + 'account/login', model).pipe(
      map((response: User) => {
        const user = response;
        if (user) {
          this.setCurrentUser(user);
        }
      })
    );
  }

  setCurrentUser(user: User) {
    localStorage.setItem('user', JSON.stringify(user));
    this.currentUserSource.next(user);
  }

  logout() {
    localStorage.removeItem('user');
    this.currentUserSource.next();
  }

  register(model: any) {
    return this.http.post<User>(this.baseUrl + 'account/register', model).pipe(
      // ℹ️ Since there is no return on the map, nothing will be returned to this observable's suscribers
      map((user: User) => {
        if (user) {
          this.setCurrentUser(user);
        }
      })
    );
  }

}
