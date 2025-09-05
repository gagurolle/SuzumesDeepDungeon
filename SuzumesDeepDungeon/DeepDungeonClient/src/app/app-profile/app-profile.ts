import { Component } from '@angular/core';
import { AuthService } from '../services/auth/auth';
import { Router } from '@angular/router';
import { ApiFormComponent } from "../api-form/api-form.component";

@Component({
  selector: 'app-profile',
  imports: [ApiFormComponent],
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