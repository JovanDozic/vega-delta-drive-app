import { Component } from '@angular/core';
import { AuthenticationService } from '../../services/authentication/authentication.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css',
})
export class NavbarComponent {
  isLoggedIn: boolean = false;
  currentUserFirstName?: string = '';

  constructor(private authService: AuthenticationService) {
    this.authService.isLoggedIn.subscribe((isLoggedIn) => {
      this.isLoggedIn = isLoggedIn;
      if (this.isLoggedIn) {
        // this.currentUserFirstName = this.authService.currentUserFirstName;
        this.currentUserFirstName = this.authService.getUserFirstName();
      }
    });
  }

  logout() {
    this.authService.logout();
    location.reload();
  }
}
