import { Component } from '@angular/core';
import { AuthenticationRequest } from '../../model/authentication/authentication-request.model';
import { AuthenticationResponse } from '../../model/authentication/authentication-response.model';
import { Router } from '@angular/router';
import { Location } from '@angular/common';
import { AuthenticationService } from '../../services/authentication/authentication.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
})
export class LoginComponent {
  authRequest: AuthenticationRequest = {};
  authResponse: AuthenticationResponse = {};

  constructor(
    private router: Router,
    private authService: AuthenticationService
  ) {}

  login() {
    this.authService.login(this.authRequest).subscribe({
      next: (response) => {
        if (response.isSuccess) {
          this.router.navigate(['/']);
        } else {
          alert('Login failed!');
        }
      },
      error: (err) => {
        alert('Login failed!');
        console.error(err);
      },
    });
  }
}
