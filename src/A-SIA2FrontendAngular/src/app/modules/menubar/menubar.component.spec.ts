import { ComponentFixture, TestBed } from '@angular/core/testing';
import { MenubarEntryComponent } from './menubar-menu/menubar-entry/menubar-entry.component';
import { MenubarMenuComponent } from './menubar-menu/menubar-menu.component';

import { MenubarComponent } from './menubar.component';
import { MenubarService } from './menubar.service';

describe('MenubarComponent', () => {
  let component: MenubarComponent;
  let fixture: ComponentFixture<MenubarComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [MenubarComponent, MenubarMenuComponent, MenubarEntryComponent],
      providers: [MenubarService]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(MenubarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
