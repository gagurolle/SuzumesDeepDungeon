import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { AuthService } from './auth/auth';


@Injectable({ providedIn: 'root' })
export class AdminGuard implements CanActivate {
  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  canActivate(): boolean {
    const user = this.authService;  // Используем геттер
    if (!user || !user.isAdmin) {
      //this.router.navigate(['/']);
      return false;
    }
    return true;
  }
}