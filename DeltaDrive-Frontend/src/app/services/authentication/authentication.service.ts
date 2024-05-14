import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AuthenticationRequest } from '../../model/authentication/authentication-request.model';
import { environment } from '../../../environments/environment';
import { Result } from '../../model/common/result.model';
import { AuthenticationResponse } from '../../model/authentication/authentication-response.model';
import {
  BehaviorSubject,
  Observable,
  catchError,
  map,
  tap,
  throwError,
} from 'rxjs';
import { RegisterRequest } from '../../model/authentication/register-request.model';
import { User } from '../../model/user.model';
import { TokenService } from './token.service';

@Injectable({
  providedIn: 'root',
})
export class AuthenticationService {
  private loggedIn = new BehaviorSubject<boolean>(this.hasToken());

  constructor(private http: HttpClient, private tokenService: TokenService) {}

  login(
    authRequest: AuthenticationRequest
  ): Observable<AuthenticationResponse> {
    return this.http
      .post<AuthenticationResponse>(
        `${environment.apiHost}/Authentication/login`,
        authRequest,
        { observe: 'response' }
      )
      .pipe(
        tap((response) => {
          if (response.status === 200 && response.body?.accessToken) {
            localStorage.setItem('token', response.body?.accessToken as string);
            this.loggedIn.next(true);
          }
        }),
        map((response) => response.body as AuthenticationResponse),
        catchError(this.handleError)
      );
  }

  register(regRequest: RegisterRequest): Observable<any> {
    return this.http
      .post<Result<any>>(
        `${environment.apiHost}/Authentication/register`,
        regRequest,
        { observe: 'response' }
      )
      .pipe(
        tap(),
        map((response) => response.body),
        catchError(this.handleError)
      );
  }

  logout() {
    localStorage.removeItem('token');
    this.loggedIn.next(false);
  }

  private handleError(error: HttpErrorResponse): Observable<never> {
    let errorMessage = 'An unknown error occurred!';
    if (error.error instanceof ErrorEvent) {
      // Client-side error
      errorMessage = `Error: ${error.error.message}`;
    } else {
      // Server-side error
      if (error.status === 401) {
        errorMessage = 'Invalid credentials';
      } else {
        errorMessage = `Error Code: ${error.status}\nMessage: ${error.message}`;
      }
    }
    return throwError(() => new Error(errorMessage));
  }

  getUserFirstName() {
    return this.tokenService.getUserDetailsFromToken().name;
  }

  getUserId() {
    return this.tokenService.getUserDetailsFromToken().id;
  }

  private hasToken(): boolean {
    return !!localStorage.getItem('token');
  }

  get isLoggedIn() {
    return this.loggedIn.asObservable();
  }
}
