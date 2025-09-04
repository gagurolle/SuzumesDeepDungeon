// game-list.component.ts
import { ChangeDetectorRef, Component, HostListener, NgZone, OnDestroy, OnInit } from '@angular/core';

import { CommonModule } from '@angular/common';
import { DatePipe } from '@angular/common';
import { GameRankDTO } from '../models/game-ranking';

import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatIconModule } from '@angular/material/icon';
import { GameAddFormComponent } from "../game-add-form/game-add-form";
import { GameStatusService } from '../services/game-status/game-status.service';
import { GameStatus } from '../models/game-status.enum';
import { AuthService } from '../services/auth/auth';
import { Router } from '@angular/router';
import { MatInputModule } from "@angular/material/input";
import { FormsModule } from '@angular/forms';
import { MatSelectModule } from '@angular/material/select';
import { MatOptionModule } from '@angular/material/core';
import { Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged, takeUntil } from 'rxjs/operators';
import { GameDetailsViewComponent } from "../game-details-view/game-details-view.component";
import { GameService } from '../services/game.service/game.service';
import { AppMenuComponent } from "../app-menu/app-menu.component";

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
    GameDetailsViewComponent,
    AppMenuComponent
],
  templateUrl: './game-list.html',
  styleUrls: ['./game-list.css'],
  providers: [DatePipe]
})
export class GameListComponent implements OnInit, OnDestroy {

  games: GameRankDTO[] = [];
  isLoading = true;
  isLoadingMore = false;
  errorMessage = '';
  showAddForm = false;
  showForm = false;
  currentGame: GameRankDTO | null = null;
  searchName: string = '';
  selectedStatus: GameStatus | null = null;
  sortField: string = 'created';
  sortDesc: boolean = true;
  selectedGame: GameRankDTO | null = null;
  page = 1;
  pageSize = 30;
  totalCount = 0;
  hasMore = true;

  private searchSubject = new Subject<string>();
  private destroy$ = new Subject<void>();

  constructor(
    private gameService: GameService,
    private datePipe: DatePipe,
    private cdr: ChangeDetectorRef,
    public statusService: GameStatusService,
    public auth: AuthService,
    private router: Router
  ) { }

  statuses: { value: GameStatus, text: string }[] = [];


  ngOnInit(): void {
    this.loadGames();
    this.statuses = this.statusService.getAllStatuses();

    this.searchSubject.pipe(
      debounceTime(500),
      distinctUntilChanged(),
      takeUntil(this.destroy$)
    ).subscribe(searchTerm => {
      this.searchName = searchTerm;
      this.page = 1;
      this.loadGames(true);
    });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  // Обработчик скролла
  @HostListener('window:scroll')
  onWindowScroll(): void {
    //if (this.isLoading || this.isLoadingMore || !this.hasMore) return;

    // Вычисляем, достигли ли мы низа страницы
    const threshold = 100; // Загружать за 100px до конца
    const position = window.scrollY + window.innerHeight;
    const height = document.body.scrollHeight;

    if (position > height - threshold) {
      this.loadMore();
    }
  }

  // Загрузка дополнительных элементов
  loadMore(): void {
    if (this.isLoadingMore || !this.hasMore) return;

    this.isLoadingMore = true;
    this.page++;
    this.loadGames(false);
  }

  openGameDetails(game: GameRankDTO): void {
    this.selectedGame = game;

    this.cdr.detectChanges();
  }
  closeGameDetails(): void {
    this.selectedGame = null;

    this.cdr.detectChanges();
  }
  onSearchInput(event: Event): void {
    const input = event.target as HTMLInputElement;
    const value = input.value.trim();


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
    this.games.unshift(newGame);
    this.closeAddForm();
  }

  onGameSaved(updatedGame: GameRankDTO): void {
    if (this.currentGame) {

      const index = this.games.findIndex(g => g.id === updatedGame.id);
      if (index !== -1) {
        this.games[index] = updatedGame;
      }
    } else {

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

  loadGames(reset: boolean = false): void {
if (reset) {
      this.page = 1;
      this.games = [];
      this.hasMore = true;
      this.isLoading = true;
    } else {
      this.isLoadingMore = true;
    }



    this.isLoading = true;
    this.errorMessage;
    const params: any = {};

    if (this.searchName) params.name = this.searchName;
    if (this.selectedStatus !== null) params.status = this.selectedStatus;
    if (this.sortField) params.sortBy = this.sortField;
    if (this.sortDesc !== undefined) params.desc = this.sortDesc;

    params.page = this.page;
    params.pageSize = this.pageSize;
    
    this.gameService.getGames(params).subscribe({
      next: (data: any) => {
        if (reset) {
          this.isLoading = false;
        } else {
          this.isLoadingMore = false;
        }
        

        this.games = [...this.games, ...data.items];
        this.totalCount = data.totalCount;
        this.hasMore = this.page < data.totalPages;
        
        
        this.cdr.markForCheck();
      },
      error: (err) => {
        this.errorMessage = 'Ошибка загрузки списка игр';
        this.isLoading = false;
        this.isLoadingMore = false;
        this.cdr.detectChanges();
        console.error(err);
      }
    });
  }

  getPlayTime(date: Date | undefined): string {
    if (!date) return '0 часов';
    return this.datePipe.transform(date, 'HH часов mm минут') || '0 часов';
  }
  formatPlayTime(isoTime: string): string {
    const parts = isoTime.split(':');
    return `${parts[0]} H`;
  }
  handleImageError(event: Event): void {
    const img = event.target as HTMLImageElement;

    if (!img.src.endsWith('default-game.jpg')) {
      img.src = 'assets/default-game.jpg';
    } else {


      img.onerror = null;
    }
  }
}