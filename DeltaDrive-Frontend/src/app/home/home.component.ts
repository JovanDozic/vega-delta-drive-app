import { Component } from '@angular/core';
import { AuthenticationService } from '../services/authentication/authentication.service';
import { Location } from '../model/location.model';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrl: './home.component.css',
})
export class HomeComponent {
  loginMessage: string = '';
  isLoggedIn: boolean = false;
  location: Location = { latitude: 45.267136, longitude: 19.833549 };

  constructor(private authService: AuthenticationService) {
    this.authService.isLoggedIn.subscribe((isLoggedIn) => {
      this.isLoggedIn = isLoggedIn;
      this.loginMessage = isLoggedIn
        ? 'You are logged in'
        : 'You are not logged in';
    });
  }

  onCoordinatesChanged(event: { lng: number; lat: number }) {
    this.location.latitude = event.lat;
    this.location.longitude = event.lng;
  }

  logout() {
    this.authService.logout();
    location.reload();
  }
}
