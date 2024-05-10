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
    private location: Location,
    private router: Router,
    private authService: AuthenticationService
  ) {}

  goBack() {
    this.location.back();
  }

  login() {
    this.authService.login(this.authRequest).subscribe({
      next: (response) => {
        if (response.isSuccess) {
          this.router.navigate(['/']);
        } else {
          alert('Login failed');
        }
      },
      error: (err) => {
        alert('An error occurred');
        console.error(err);
      },
    });
  }
}
