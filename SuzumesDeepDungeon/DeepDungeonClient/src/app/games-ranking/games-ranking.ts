import { Component, OnInit } from '@angular/core';
import { GameRankDTO } from '../models/game-ranking';
import { GameService } from '../api/game.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-games-ranking',
  templateUrl: './games-ranking.html',
  styleUrl: './games-ranking.css',
})
export class GamesRanking implements OnInit {
  games: GameRankDTO[] = [];
    constructor(private gameService: GameService) {}

  ngOnInit(): void {
    this.loadGames();
  }

  loadGames(): void {
    this.gameService.getGames().subscribe({
      next: (data) => this.games = data,
      error: (err) => console.error('Ошибка загрузки:', err)
    });
  }
}

 
  

