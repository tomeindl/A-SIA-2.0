import { ComponentFixture, TestBed } from '@angular/core/testing';
import { MenubarEntryComponent } from './menubar-entry/menubar-entry.component';
import { MenubarService } from '../menubar.service';

import { MenubarMenuComponent } from './menubar-menu.component';

describe('MenubarMenuComponent', () => {
  let component1: MenubarMenuComponent;
  let component2: MenubarMenuComponent;
  let fixture1: ComponentFixture<MenubarMenuComponent>;
  let fixture2: ComponentFixture<MenubarMenuComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [MenubarMenuComponent, MenubarEntryComponent],
      providers: [MenubarService]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture1 = TestBed.createComponent(MenubarMenuComponent);
    component1 = fixture1.componentInstance;
    fixture2 = TestBed.createComponent(MenubarMenuComponent);
    component2 = fixture2.componentInstance;
    fixture2.detectChanges();
    fixture1.detectChanges();
  });

  it('should create', () => {
    expect(component1).toBeTruthy();
  });

  it('should be closed', () => {
    expect(component1.isOpen).toBeFalse();
  });

  it('should click open', () => {
    component1.onLabelClick();
    expect(component1.isOpen).toBeTrue();
  });
  it('should click close', () => {
    component1.onLabelClick();
    component1.onLabelClick();
    expect(component1.isOpen).toBeFalse();
  });

  it('should hover close', () => {
    component1.onLabelMouseEnter();
    expect(component1.isOpen).toBeFalse();
  });
  it('should close others on hover', () => {
    component1.onLabelClick();
    component2.onLabelMouseEnter();
    expect(component1.isOpen).toBeFalse();
  });
  it('should hover open', () => {
    component1.onLabelClick();
    component2.onLabelMouseEnter();
    expect(component2.isOpen).toBeTrue();
  });
});
