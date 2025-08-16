import { Component } from '@angular/core';
import { AuthService } from '../auth/auth';
import { Router } from '@angular/router';

@Component({
  selector: 'app-profile',
  imports: [],
  templateUrl: './app-profile.html',
  styleUrl: './app-profile.css'
})
export class ProfileComponent {
  constructor(public auth: AuthService, private router: Router) {}

  logout() {
    this.auth.logout();
    this.router.navigate(['/']);
  }

  goHome() {
    this.router.navigate(['/']);
  }
}