import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { GamesRanking } from './games-ranking/games-ranking';
import { GameListComponent } from './game-list/game-list';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { LoginComponent } from "./login/login";

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, ReactiveFormsModule, FormsModule],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected readonly title = signal('DeepDungeonClient');
}
