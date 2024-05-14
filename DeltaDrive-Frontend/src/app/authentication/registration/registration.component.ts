import { Component } from '@angular/core';
import { RegisterRequest } from '../../model/authentication/register-request.model';
import { Location } from '@angular/common';
import { Router } from '@angular/router';
import { AuthenticationService } from '../../services/authentication/authentication.service';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrl: './registration.component.css',
})
export class RegistrationComponent {
  regRequest: RegisterRequest = {};

  constructor(
    private router: Router,
    private authService: AuthenticationService
  ) {}

  register() {
    this.authService.register(this.regRequest).subscribe({
      next: (response: any) => {
        if (response) {
          alert('Account created successfully!');
          this.router.navigate(['/']);
        } else {
          alert('Failed to create account: Invalid response from server');
        }
      },
      error: (error) => {
        console.log('Error: ' + error.message);
        alert('Oops! Validation error! Check your inputs and try again.');
      },
    });
  }
}
