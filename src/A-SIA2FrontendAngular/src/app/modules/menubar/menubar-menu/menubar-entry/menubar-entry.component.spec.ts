import { ComponentFixture, TestBed } from '@angular/core/testing';
import { MenubarService } from '../../menubar.service';

import { MenubarEntryComponent } from './menubar-entry.component';

describe('MenubarEntryComponent', () => {
  let component: MenubarEntryComponent;
  let fixture: ComponentFixture<MenubarEntryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [MenubarEntryComponent],
      providers: [MenubarService]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(MenubarEntryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should invoke function', () => {
    let valueToChange = 10;
    component.action = () => {
      valueToChange = 20;
    };
    component.onClick();
    expect(valueToChange).toBe(20);
  });
});
