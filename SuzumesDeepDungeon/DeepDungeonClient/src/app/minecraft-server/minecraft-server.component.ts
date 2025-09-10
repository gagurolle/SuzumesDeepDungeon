import { ChangeDetectorRef, Component, OnInit } from "@angular/core";
import { MinecraftContent } from "../models/minecraft-content";
import { MinecraftMainContent } from "../models/minecraft-main-content";
import { MinecraftService } from "../services/minecraft-service/minecraft-service.service";
import { FormatTextPipe } from "../Pipes/format-text-pipe";
import { FormsModule } from "@angular/forms";
import { CommonModule } from "@angular/common";
import { AuthService } from "../services/auth/auth";
import { User } from "../models/user";

@Component({
  selector: 'app-minecraft-server',
  templateUrl: './minecraft-server.component.html',
  styleUrls: ['./minecraft-server.component.css'],
  imports: [FormatTextPipe, FormsModule, CommonModule]
})
export class MinecraftServerComponent implements OnInit {
  mainContent: MinecraftMainContent = {
    header: '',
    headerInfo: '',
    version: '',
    mod: '',
    adres: ''
  };
  isEditingMain = false;
  
  contents: MinecraftContent[] = [];
  currentPage = 1;
  pageSize = 4;
  totalCount = 0;
  totalPages = 0;
  
  isAddingContent = false;
  newContent: Partial<MinecraftContent> = {};
  user: User | null = null;
   showAddress: boolean = false;

  // Новые свойства для управления редактированием карточек
  editingContentId: number | null = null;
  editContentData: MinecraftContent | null = null;

  constructor(
    private minecraftService: MinecraftService, 
    public auth: AuthService, 
    private cdr: ChangeDetectorRef
  ) { }

  ngOnInit(): void {
    this.loadMainContent();
    this.loadContents();
    this.user = this.auth.checkUser();
    this.mainContent.user = this.user || undefined;
    this.cdr.markForCheck();
  }

  loadMainContent(): void {
    this.minecraftService.getMainContent().subscribe({
      next: (data) => {
        if (data) {
          this.mainContent = data;
          this.cdr.markForCheck();
        }
      },
      error: (err) => {
        console.error('Ошибка загрузки основного контента:', err);
      }
    });
  }

  loadContents(): void {
    this.minecraftService.getContents(this.currentPage, this.pageSize).subscribe({
      next: (response: PagedResponse<MinecraftContent>) => {
        this.contents = response.items;
        this.totalCount = response.totalCount;
        this.totalPages = response.totalPages;
        this.cdr.markForCheck();
      },
      error: (err) => {
        console.error('Ошибка загрузки контента:', err);
      }
    });
  }

  toggleEditMain(): void {
    this.isEditingMain = !this.isEditingMain;
  }

  saveMainContent(): void {
    this.mainContent.user = this.user || undefined;
    this.minecraftService.updateMainContent(this.mainContent).subscribe({
      next: (data) => {
        this.mainContent = data;
        this.isEditingMain = false;
        this.cdr.markForCheck();
      },
      error: (err) => {
        console.error('Ошибка сохранения основного контента:', err);
      }
    });
  }

  toggleAddContent(): void {
    this.isAddingContent = !this.isAddingContent;
    if (this.isAddingContent) {
      this.newContent = {};
    }
  }

  addContent(): void {
    if (this.newContent.header && this.newContent.content) {
      this.newContent.user = this.user || undefined;
      this.minecraftService.createContent(this.newContent as MinecraftContent).subscribe({
        next: (data) => {
          this.contents.unshift(data);
          this.isAddingContent = false;
          this.newContent = {};
          this.loadContents();
          this.cdr.markForCheck();
        },
        error: (err) => {
          console.error('Ошибка добавления контента:', err);
        }
      });
    }
  }

  // Новые методы для редактирования карточек
  startEditContent(content: MinecraftContent): void {
    this.editingContentId = content.id!;
    this.editContentData = { ...content };
  }

  cancelEditContent(): void {
    this.editingContentId = null;
    this.editContentData = null;
  }

  saveContent(): void {
    if (this.editContentData) {
      this.minecraftService.updateContent(this.editContentData).subscribe({
        next: (data) => {
          const index = this.contents.findIndex(c => c.id === data.id);
          if (index !== -1) {
            this.contents[index] = data;
          }
          this.cancelEditContent();
          this.cdr.markForCheck();
        },
        error: (err) => {
          console.error('Ошибка редактирования контента:', err);
        }
      });
    }
  }

  deleteContent(id: number): void {
    if (confirm('Вы уверены, что хотите удалить эту запись?')) {
      this.minecraftService.deleteContent(id).subscribe({
        next: () => {
          this.contents = this.contents.filter(c => c.id !== id);
          this.loadContents();
        },
        error: (err) => {
          console.error('Ошибка удаления контента:', err);
        }
      });
    }
  }

  changePage(page: number): void {
    this.currentPage = page;
    this.loadContents();
  }

  getPagesArray(): number[] {
    return Array(this.totalPages).fill(0).map((x, i) => i);
  }

  toggleAddressVisibility(): void {
    this.showAddress = !this.showAddress;
  }
  copyToClipboard(text: string): void {
  navigator.clipboard.writeText(text).then(() => {
    console.log('Адрес скопирован в буфер обмена');
    // Можно добавить уведомление пользователю
  }).catch(err => {
    console.error('Ошибка копирования: ', err);
  });
}
  getObfuscatedAddress(): string {
  if (!this.mainContent.adres) return 'Не указан';
  
  if (this.showAddress) {
    return this.mainContent.adres;
  } else {
    // Возвращаем замаскированный адрес
    return this.mainContent.adres.replace(/./g, '•');
  }
}
}