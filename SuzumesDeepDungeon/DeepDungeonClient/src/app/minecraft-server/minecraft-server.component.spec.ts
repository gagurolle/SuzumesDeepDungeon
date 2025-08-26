import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MinecraftServerComponent } from './minecraft-server.component';

describe('MinecraftServerComponent', () => {
  let component: MinecraftServerComponent;
  let fixture: ComponentFixture<MinecraftServerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MinecraftServerComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MinecraftServerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
