import { Component } from '@angular/core';
import { AuthenticationService } from '../services/authentication/authentication.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrl: './home.component.css',
})
export class HomeComponent {
  loginMessage: string = '';
  isLoggedIn: boolean = false;

  constructor(private authService: AuthenticationService) {
    this.authService.isLoggedIn.subscribe((isLoggedIn) => {
      this.isLoggedIn = isLoggedIn;
      this.loginMessage = isLoggedIn
        ? 'You are logged in'
        : 'You are not logged in';
    });
  }

  logout() {
    this.authService.logout();
    location.reload();
  }

  testJWT() {
    this.authService.testJWT().subscribe((response) => {
      console.log(response);
    });
  }
}
