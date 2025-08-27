import { Component } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';

@Component({
  selector: 'app-donate-links',
  standalone: true,
imports: [MatCardModule, MatButtonModule],
  templateUrl: './donate-links.component.html',
  styleUrl: './donate-links.component.css'
})
export class DonateLinksComponent {

}
