import { Component, Input, Output, EventEmitter, OnInit } from "@angular/core";
import { FormGroup, FormBuilder, Validators, ReactiveFormsModule } from "@angular/forms";
import { GameRankDTO } from "../models/game-ranking";
import { CommonModule } from "@angular/common";
import { GameStatusService } from "../services/game-status/game-status.service";
import { GameStatus } from "../models/game-status.enum";
import { GameFindComponent } from "../game-find/game-find.component";
import { AuthService } from "../services/auth/auth";
import { User } from "../models/user";
import { DomSanitizer, SafeResourceUrl } from "@angular/platform-browser";
import { GameService } from "../services/game.service/game.service";

@Component({
  selector: 'app-game-add-form',
  templateUrl: './game-add-form.html',
  styleUrls: ['./game-add-form.css'],
  imports: [ReactiveFormsModule, CommonModule, GameFindComponent]
})
export class GameAddFormComponent implements OnInit {
  @Input() gameToEdit: GameRankDTO | null = null;
  @Output() gameSaved = new EventEmitter<GameRankDTO>();
  @Output() gameDeleted = new EventEmitter<number>();
  @Output() closed = new EventEmitter<void>();
  @Output() gameAdded = new EventEmitter<GameRankDTO>();

  gameForm: FormGroup;
  isEditMode = false;
  errorMessage: string = '';
  isSubmitting: boolean = false;
  gameStatuses: { value: GameStatus, text: string }[] = [];
  public currentGameName: string = '';
  foundGame: GameRankDTO | null = null;
  user: User | null = null;
  constructor(
    private fb: FormBuilder,
    private gameService: GameService,
    public statusService: GameStatusService,
    public auth: AuthService,
    private sanitizer: DomSanitizer
  ) {

    this.gameForm = this.fb.group({
      name: ['', Validators.required],
      rate: [null, [Validators.min(0), Validators.max(10)]],
      status: [GameStatus.Unknown, Validators.required],
      gameTime: ['0', Validators.required],
      review: [''],
      image: [''],
      youtubeLink: [''],
      metacriticRate: [null],
      released: [null],


    });
    this.gameStatuses = this.statusService.getAllStatuses();
    this.gameForm.get('name')?.valueChanges.subscribe(value => {
      this.currentGameName = value;
    });
  }

  ngOnInit(): void {

    this.user = this.auth.checkUser();
  }
  ngOnChanges(): void {

    if (this.gameToEdit) {
      this.isEditMode = true;
      this.gameForm.patchValue({
        id: this.gameToEdit.id,
        name: this.gameToEdit.name,
        rate: this.gameToEdit.rate,
        status: this.gameToEdit.status,
        gameTime: this.gameToEdit.gameTime,
        review: this.gameToEdit.review,
        image: this.gameToEdit.image,
        youtubeLink: this.gameToEdit.youtubeLink,
        metacriticRate: this.gameToEdit.metacriticRate,
        released: this.gameToEdit.released ? new Date(this.gameToEdit.released).toISOString().split('T')[0] : null
      });
    }
  }

  onSave(): void {

    if (this.gameForm.invalid || this.isSubmitting) return;

    this.isSubmitting = true;
    this.errorMessage = '';





    if (this.isEditMode && this.gameToEdit?.id) {
      if (this.foundGame == null) {
        const gameData = {
          
          ...this.gameToEdit,
          ...this.gameForm.value,
          id: this.gameToEdit?.id,
          status: this.gameForm.value.status as GameStatus,
          gameTime: this.formatGameTime(this.gameForm.value.gameTime),
          user: this.user,
          rate: this.gameForm.value.rate || 0,
        };
        this.gameService.updateGame(gameData).subscribe({
          next: (updatedGame) => {
            this.gameSaved.emit(updatedGame);
            this.gameToEdit = updatedGame;
            this.close();
          },
          error: (err) => console.error(err)
        });

      } else {
        const gameData = {
          ...this.gameForm.value,
          ...this.foundGame,
        rate: this.gameForm.value.rate,
        released: this.gameForm.value.released ? new Date(this.gameForm.value.released).toISOString().split('T')[0] : null,
        image:  this.gameForm.value.image || this.foundGame.image,
        metacriticRate: this.gameForm.value.metacriticRate || this.foundGame.metacriticRate,
        id: this.gameToEdit?.id,
        status: this.gameForm.value.status as GameStatus,
        gameTime: this.formatGameTime(this.gameForm.value.gameTime),
        review: this.gameForm.value.review,
        name: this.gameForm.value.name,
        youtubeLink: this.gameForm.value.youtubeLink,
        user: this.user
        };
        this.gameService.updateGame(gameData).subscribe({
          next: (updatedGame) => {
            this.gameSaved.emit(updatedGame);
            this.gameToEdit = updatedGame;
            this.close();
            this.foundGame = null; 
          },
          error: (err) => console.error(err)
        });
      }
    }
    else {
      const gameDataPatch = {
        ...this.gameForm.value,
        ...this.gameToEdit,
        id: this.gameToEdit?.id,
        status: this.gameForm.value.status as GameStatus,
        gameTime: this.gameForm.value.gameTime,
        review: this.gameForm.value.review,
        youtubeLink: this.gameForm.value.youtubeLink,
        rate: this.gameForm.value.rate,
        user: this.user
      }
      this.gameService.addGame(gameDataPatch).subscribe({
        next: (newGame) => {
          this.errorMessage = '';
          this.isSubmitting = false;
          this.gameSaved.emit(newGame);
          this.foundGame = null; 
          this.close();
        },
        error: (err) => {
          this.isSubmitting = false;
          this.errorMessage = 'Ошибка при добавлении игры';
          console.error(err)
        }
      });
    }
  }
  showDeleteConfirm = false;

onGameFound(foundGame: GameRankDTO) {
if(this.gameToEdit){
  this.gameToEdit.name = foundGame.name;
  this.gameToEdit.released = foundGame.released;
  this.gameToEdit.image = foundGame.image || this.gameForm.value.image;
  this.gameToEdit.metacriticRate = foundGame.metacriticRate || null;
  this.gameToEdit.stores = foundGame.stores || [];
  this.gameToEdit.screenshots = foundGame.screenshots;;
  this.gameToEdit.trailers = foundGame.trailers || [];
  this.gameToEdit.tags = foundGame.tags || [];
  this.gameToEdit.achievements = foundGame.achievements || [];
  this.gameToEdit.rawgId = foundGame.rawgId || null;
}
else{
  this.gameToEdit = {
    ...foundGame,
    name: foundGame.name,
    status: this.gameForm.value.status as GameStatus,
    user: this.user,
    rate: this.gameForm.value.rate || 0,
    gameTime:  this.gameForm.value.gameTime,
    youtubeLink: this.gameForm.value.youtubeLink,
    review: this.gameForm.value.review,
  };
}

const gameDataPatch = {
  ...this.gameToEdit};
  this.gameForm.patchValue(gameDataPatch);
}


  onGameFound2s(foundGame: GameRankDTO) {

    const gameDataPatch = {
          ...this.foundGame,
        name: foundGame.name,
        id: this.gameToEdit?.id,
        status: this.gameForm.value.status as GameStatus,
        gameTime: this.formatGameTime(this.gameForm.value.gameTime),
        user: this.user,
        rate: this.gameForm.value.rate,
        released: foundGame.released,
        image: foundGame.image || this.gameForm.value.image,
        metacriticRate: foundGame.metacriticRate || null,
        stores: foundGame.stores || [],
        screenshots: foundGame.screenshots || [],
        trailers: foundGame.trailers || [],
        youtubeLink: foundGame.youtubeLink || this.gameForm.value.youtubeLink,
        review: foundGame.review || this.gameForm.value.review,
        tags: foundGame.tags || [],
        achievements: foundGame.achievements || [],
        rawgId: foundGame.rawgId || null
        
      }
    this.gameForm.patchValue(gameDataPatch);
    this.foundGame = foundGame;
    this.gameToEdit = {...foundGame,
        name: foundGame.name,
        status: this.gameForm.value.status as GameStatus,
        gameTime: 0,
        user: this.user,
        rate: this.gameForm.value.rate,
        youtubeLink: foundGame.youtubeLink || this.gameForm.value.youtubeLink,
        review: foundGame.review || this.gameForm.value.review,

    }
  }

  confirmDelete(): void {
    this.showDeleteConfirm = true;
  }

  onDelete(): void {

    if (this.gameToEdit?.id) {
      this.gameService.deleteGame(this.gameToEdit.id).subscribe({
        next: () => {
          this.gameDeleted.emit(this.gameToEdit!.id);
          this.close();
        },
        error: (err) => console.error(err)
      });
    }
  }

  close(): void {
    this.gameForm.reset();
    this.isEditMode = false;
    this.closed.emit();
  }

  getSafeUrl(url: string): SafeResourceUrl {
    return this.sanitizer.bypassSecurityTrustResourceUrl(url);
  }
//TODO
  private formatGameTime(timeInput: any): string {
    return timeInput;
  }
}