import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AuthenticationRequest } from '../../model/authentication/authentication-request.model';
import { environment } from '../../../environments/environment';
import { Result } from '../../model/common/result.model';
import { AuthenticationResponse } from '../../model/authentication/authentication-response.model';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { RegisterRequest } from '../../model/authentication/register-request.model';

@Injectable({
  providedIn: 'root',
})
export class AuthenticationService {
  private loggedIn = new BehaviorSubject<boolean>(this.hasToken());

  constructor(private http: HttpClient) {}

  login(
    authRequest: AuthenticationRequest
  ): Observable<Result<AuthenticationResponse>> {
    return this.http
      .post<Result<AuthenticationResponse>>(
        `${environment.apiHost}/Authentication/login`,
        authRequest
      )
      .pipe(
        tap((response) => {
          if (response.isSuccess && response.value?.accessToken) {
            localStorage.setItem('token', response.value.accessToken as string);
            this.loggedIn.next(true);
          }
        })
      );
  }

  register(regRequest: RegisterRequest) {
    return this.http
      .post<Result<any>>(
        `${environment.apiHost}/Authentication/register`,
        regRequest
      )
      .pipe(
        tap((response) => {
          // if (response.isSuccess) {
          //   this.loggedIn.next(true);
          // }
        })
      );
  }

  logout() {
    localStorage.removeItem('token');
    this.loggedIn.next(false);
  }

  private hasToken(): boolean {
    return !!localStorage.getItem('token');
  }

  get isLoggedIn() {
    return this.loggedIn.asObservable();
  }
}
