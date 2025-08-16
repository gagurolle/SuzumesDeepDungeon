import { Component, EventEmitter, Input, Output } from '@angular/core';
import { GameFindServiceService } from '../game-find-service.service';
import { FindGameDTO } from '../models/game-find';
import { GameRankDTO } from '../models/game-ranking';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-game-find',
  standalone: true,
  imports: [CommonModule, MatButtonModule],
  templateUrl: './game-find.component.html',
  styleUrls: ['./game-find.component.css']
})
export class GameFindComponent {
  @Input() gameName: string = '';
  @Output() gameFound = new EventEmitter<GameRankDTO>();

  games: FindGameDTO[] = [];
  isLoading = false;
  errorMessage: string | null = null;

  constructor(private findService: GameFindServiceService) {}

  async findGame(): Promise<void> {
    if (!this.gameName.trim()) {
      this.errorMessage = 'Введите название игры для поиска';
      return;
    }

    this.isLoading = true;
    this.errorMessage = null;
    this.games = [];

    try {
      // Используем setTimeout для гарантированного обновления UI перед долгой операцией
      await new Promise(resolve => setTimeout(resolve, 0));
      
      const data = await this.findService.getGames(this.gameName).toPromise();
      this.games = data || [];
      
      if (this.games.length === 0) {
        this.errorMessage = 'Игры не найдены';
      }
    } catch (err) {
      console.error(err);
      this.errorMessage = 'Ошибка при поиске игр';
    } finally {
      this.isLoading = false;
    }
  }

  async chooseGame(game: FindGameDTO): Promise<void> {
    this.isLoading = true;
    this.errorMessage = null;

    try {
      await new Promise(resolve => setTimeout(resolve, 0));
      
      const data = await this.findService.getGameData(game).toPromise();
      if (data) {
        this.gameFound.emit(data);
      }
    } catch (err) {
      console.error(err);
      this.errorMessage = 'Ошибка при загрузке данных игры';
    } finally {
      this.games = []; 
      this.isLoading = false;
    }
  }
}