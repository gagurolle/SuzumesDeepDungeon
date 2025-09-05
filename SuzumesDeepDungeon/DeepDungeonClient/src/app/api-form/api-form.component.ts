import { ChangeDetectorRef, Component, Input } from '@angular/core';
import { ExternalApi } from '../models/external-api';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ApiKeysService } from '../services/api-keys/api-keys.service';
import { User } from '../models/user';
import {MatSlideToggleModule} from '@angular/material/slide-toggle';

@Component({
  selector: 'app-api-form',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule, MatSlideToggleModule],
  templateUrl: './api-form.component.html',
  styleUrl: './api-form.component.css'
})
export class ApiFormComponent {

public apiForm: FormGroup;
public apiKeys: ExternalApi[] = [];
  isSubmitting: boolean = false;
  public showPage = false;
  @Input() username: string = null!;

constructor(private fb: FormBuilder, private apiKeysService: ApiKeysService, private cdr: ChangeDetectorRef,) {
    this.apiForm = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(3)]],
      description: ['']
    });
  }

ngOnInit(): void {
    this.loadApiKeys();
  }

  copyToClipboard(key: string | undefined): void {
    if (!key) return;
    
    navigator.clipboard.writeText(key).then(() => {
      // Можно добавить уведомление об успешном копировании
      console.log('API ключ скопирован в буфер обмена');
    });
  }

  loadApiKeys(): void {
  if(!this.username) return;
    this.apiKeysService.getApiKeys(this.username).subscribe({
      next: (keys) => {
        this.apiKeys = [...keys];
        this.cdr.markForCheck();
      },
      error: (err) => {
        console.error('Error loading API keys', err);
      }
    });
  }

  onSubmit(): void {
    if (this.apiForm.valid && !this.isSubmitting) {
      this.isSubmitting = true;
      this.apiKeysService.addNewApiKey({
        name: this.apiForm.value.name,
        description: this.apiForm.value.description,
        user: { username: this.username } as User
      }).subscribe({
        next: (newKey) => {
          this.apiKeys.push(newKey);
          this.apiForm.reset();
          this.isSubmitting = false;
          this.cdr.markForCheck();
        },
        error: (err) => {
          console.error('Error adding API key', err);
          this.isSubmitting = false;
          this.cdr.markForCheck();
        }
      });
    }
  }

  deleteApiKey(keyId: number | undefined): void {
    if (keyId && confirm('Are you sure you want to delete this API key?')) {
      this.apiKeysService.deleteApiKey(keyId).subscribe({
        next: () => {
          this.apiKeys = this.apiKeys.filter(k => k.id !== keyId);  
          this.cdr.markForCheck();
        },
        error: (err) => {
          console.error('Error deleting API key', err);
        }
      });
    }
  }
}




