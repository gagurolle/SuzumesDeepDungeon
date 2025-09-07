import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../services/auth/auth';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-app-menu',
  standalone: true,
  imports: [RouterModule, CommonModule],
  templateUrl: './app-menu.component.html',
  styleUrl: './app-menu.component.css'
})
export class AppMenuComponent {
  activePage: string = 'main';

  constructor(private router: Router, public auth: AuthService,) {}


   goToProfile(): void {
    this.router.navigate(['/profile']);
  }

  navigateToMinecraft() {
  this.router.navigate(['/minecraft']);
}

navigateToDonateLink() {
  this.router.navigate(['/donate-link']);
}

navigateToGameListFollowers() {
  this.router.navigate(['/twitch/gamelist']);
}


  isActive(page: string): boolean {
    return this.activePage === page;
  }
}
