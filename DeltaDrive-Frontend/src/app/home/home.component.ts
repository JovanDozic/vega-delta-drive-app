import { Component } from '@angular/core';
import { AuthenticationService } from '../services/authentication/authentication.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrl: './home.component.css',
})
export class HomeComponent {
  loginMessage: string = '';

  constructor(private authService: AuthenticationService) {
    this.authService.isLoggedIn.subscribe((isLoggedIn) => {
      this.loginMessage = isLoggedIn
        ? 'You are logged in'
        : 'You are not logged in';
    });
  }
}
