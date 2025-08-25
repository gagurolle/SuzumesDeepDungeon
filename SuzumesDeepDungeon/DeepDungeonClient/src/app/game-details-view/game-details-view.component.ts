import { Component, Input, Output, EventEmitter, OnInit, ChangeDetectorRef } from '@angular/core';
import { GameRankDTO } from '../models/game-ranking';
import { GameStatusService } from '../services/game-status/game-status.service';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { Store, StoresEnum } from '../models/store';
import { MatIconModule } from '@angular/material/icon';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatOptionModule } from '@angular/material/core';
import { MatInputModule } from '@angular/material/input';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSelectModule } from '@angular/material/select';
import { GameAddFormComponent } from '../game-add-form/game-add-form';
import {MatProgressBarModule} from '@angular/material/progress-bar';
import { GameService } from '../services/game.service/game.service';

@Component({
  selector: 'app-game-details-view',
  templateUrl: './game-details-view.component.html',
  styleUrls: ['./game-details-view.component.css'],
  imports: [
    CommonModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatButtonModule,
    MatInputModule,
    FormsModule,
    MatSelectModule,
    MatOptionModule,
    MatProgressBarModule
],
})
export class GameDetailsViewComponent implements OnInit{
  @Input() game!: GameRankDTO;
  @Output() close = new EventEmitter<void>();

  showAchievements = false;

  constructor(
    public statusService: GameStatusService,
    private sanitizer: DomSanitizer,
    private gameService: GameService,
    private cdr: ChangeDetectorRef,
  ) {}

  ngOnInit(): void {
    this.gameService.getGame(this.game.id).subscribe({
      next: (game) => {
        this.game = game;
        this.cdr.markForCheck();
      },
      error: (err) => console.error(err)  
    });
  }

  getStatusClass(status: string): string {
    return `status-${status}`;
  }

  getSafeUrl(url: string): SafeResourceUrl {
    return this.sanitizer.bypassSecurityTrustResourceUrl(url);
  }

  formatPlayTime(gameTime: number | null): string {
    if (!gameTime) return '0 часов';
    return `${Math.floor(gameTime)} ч ${Math.round((gameTime % 1) * 60)} мин`;
  }

  toggleAchievements(): void {
    this.showAchievements = !this.showAchievements;
  }
  
}