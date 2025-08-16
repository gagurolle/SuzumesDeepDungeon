// game-list.component.ts
import { ChangeDetectorRef, Component, NgZone, OnDestroy, OnInit } from '@angular/core';

import { CommonModule } from '@angular/common';
import { DatePipe } from '@angular/common';
import { GameRankDTO } from '../models/game-ranking';
import { GameService } from '../api/game.service';
import {MatButtonModule} from '@angular/material/button';
import {MatProgressSpinnerModule} from '@angular/material/progress-spinner';
import {MatIconModule} from '@angular/material/icon';
import { GameAddFormComponent } from "../game-add-form/game-add-form";
import { GameStatusService } from '../services/game-status.service';
import { GameStatus } from '../models/game-status.enum';
import { AuthService } from '../auth/auth';
import { Router } from '@angular/router';
import { Store, StoresEnum } from '../models/store';
import { MatInputModule } from "@angular/material/input";
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatSelectModule } from '@angular/material/select';
import { MatOptionModule } from '@angular/material/core';
import { Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged, takeUntil } from 'rxjs/operators';
import { GameDetailsViewComponent } from "../game-details-view/game-details-view.component";

@Component({
  selector: 'app-game-list',
  standalone: true,
  imports: [
    CommonModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatButtonModule,
    GameAddFormComponent,
    MatInputModule,
    FormsModule,
    MatSelectModule,
    MatOptionModule,
    GameDetailsViewComponent
],
  templateUrl: './game-list.html',
  styleUrls: ['./game-list.css'],
  providers: [DatePipe]
})
export class GameListComponent implements OnInit, OnDestroy {
 
  games: GameRankDTO[] = [];
  isLoading = true;
  errorMessage = '';
  showAddForm = false;
  showForm = false;
  currentGame: GameRankDTO | null = null;
  searchName: string = '';
  selectedStatus: GameStatus | null = null;
  sortField: string = 'created'; // Поле сортировки по умолчанию
  sortDesc: boolean = true;      // Направление сортировки по умолчанию
  selectedGame: GameRankDTO | null = null;

  private searchSubject = new Subject<string>();
  private destroy$ = new Subject<void>();

  constructor(
    private gameService: GameService,
    private datePipe: DatePipe,
    private cdr: ChangeDetectorRef,
    private zone: NgZone,
    public statusService: GameStatusService,
    public auth: AuthService,
    private router: Router
  ) {}

   statuses: { value: GameStatus, text: string }[] = [];


  ngOnInit(): void {
    this.loadGames();
    this.statuses = this.statusService.getAllStatuses();

     this.searchSubject.pipe(
      debounceTime(500),          // Задержка 500 мс
      distinctUntilChanged(),     // Игнорировать повторяющиеся значения
      takeUntil(this.destroy$)    // Автоматическая отписка при уничтожении
    ).subscribe(searchTerm => {
      this.searchName = searchTerm;
      this.loadGames();
    });
  }

 ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

openGameDetails(game: GameRankDTO): void {
    this.selectedGame = game;
    // Принудительное обновление представления
    this.cdr.detectChanges();
  }
closeGameDetails(): void {
    this.selectedGame = null;
    // Принудительное обновление представления
    this.cdr.detectChanges();
  }
  onSearchInput(event: Event): void {
    const input = event.target as HTMLInputElement;
    const value = input.value.trim();
    
    // Если введено меньше 2 символов - очищаем поиск
    if (value.length < 2) {
      if (this.searchName) {
        this.searchName = '';
        this.loadGames();
      }
      return;
    }
    
    this.searchSubject.next(value);
  }

  openAddForm(): void {
    this.currentGame = null;
    this.showForm = true;
  }

  openEditForm(game: GameRankDTO): void {
    this.currentGame = game;
    this.showForm = true;
  }

  closeAddForm(): void {
    this.showAddForm = false;
  }

  onGameAdded(newGame: GameRankDTO): void {
    this.games.unshift(newGame); // Добавляем новую игру в начало списка
    this.closeAddForm();
  }

  onGameSaved(updatedGame: GameRankDTO): void {
    if (this.currentGame) {
      // Обновляем существующую игру
      const index = this.games.findIndex(g => g.id === updatedGame.id);
      if (index !== -1) {
        this.games[index] = updatedGame;
      }
    } else {
      // Добавляем новую игру
      this.games.unshift(updatedGame);
    }
    this.showForm = false;
  }

  onGameDeleted(gameId: number): void {
    this.games = this.games.filter(game => game.id !== gameId);
    this.showForm = false;
  }

 addNewGame(newGame: GameRankDTO): void {

    this.games = [{
      ...newGame,
      rate: newGame.rate || 0,
      status: newGame.status,
      gameTime: newGame.gameTime || 0,
      created: newGame.created || new Date(),
      updated: newGame.updated || new Date(),
      image: newGame.image || 'assets/default-game.jpg',
    }, ...this.games];
  }

  loadGames(): void {
    this.isLoading = true;
    this.errorMessage;
     const params: any = {};

    if (this.searchName) params.name = this.searchName;
    if (this.selectedStatus !== null) params.status = this.selectedStatus;
    if (this.sortField) params.sortBy = this.sortField;
    if (this.sortDesc !== undefined) params.desc = this.sortDesc;

    this.gameService.getGames(params).subscribe({
      next: (data) => {
        this.isLoading = false;
        this.games = data;
        
        
        //this.cdr.detectChanges();
        this.cdr.markForCheck(); // Принудительный запуск проверки
      },
      error: (err) => {
        this.errorMessage = 'Ошибка загрузки списка игр';
        this.isLoading = false;
        this.cdr.detectChanges();
        console.error(err);
      }
    });
  }


  goToProfile(): void {
    this.router.navigate(['/profile']);
    }

  getPlayTime(date: Date | undefined): string {
    if (!date) return '0 часов';
    
    // Ваша логика преобразования Date в строку времени
    // Например: "45 часов 30 минут"
    return this.datePipe.transform(date, 'HH часов mm минут') || '0 часов';
  }
formatPlayTime(isoTime: string): string {
  const parts = isoTime.split(':');
  return `${parts[0]} H`; // "45h 30m"
}
  handleImageError(event: Event): void {
  const img = event.target as HTMLImageElement;
  // Проверяем, не пытаемся ли мы уже загрузить fallback
  if (!img.src.endsWith('default-game.jpg')) {
    img.src = 'assets/default-game.jpg';
  } else {

    
    img.onerror = null;
    // Можно скрыть изображение
    //img.style.display = 'none';
  }
}
}