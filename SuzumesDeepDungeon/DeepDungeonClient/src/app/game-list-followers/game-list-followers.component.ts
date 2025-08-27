import { Component } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';

@Component({
  selector: 'app-game-list-followers',
  standalone: true,
  imports: [MatCardModule, MatButtonModule],
  templateUrl: './game-list-followers.component.html',
  styleUrl: './game-list-followers.component.css'
})
export class GameListFollowersComponent {

}
