import { Component, ElementRef, QueryList, ViewChildren } from '@angular/core';
import { PaneService } from 'src/app/pane.service';
import { InputService } from 'src/app/shared/input.service';
import { ModalService } from 'src/app/shared/modal/modal.service';
import { MenubarMenuComponent } from './menubar-menu/menubar-menu.component';
import { MenubarService } from './menubar.service';

/**
 * Top level component for the menubar.
 * it contains menubar-menus (file, view), which in turn contain menubar-entries (save, open).
 * @example
 * ```html
 * <!-- Text within components html selectors will replace their respective <ng-Content> -->
 * <a-sia-menubar-menu label="LABEL NAME 1">
 *   <a-sia-menubar-entry [action]="ONCLICK CALLBACK FOR ENTRY">ENTRY NAME 1</a-sia-menubar-entry>
 *   <a-sia-menubar-entry [action]="ONCLICK CALLBACK FOR ENTRY">ENTRY NAME 2</a-sia-menubar-entry>
 * </a-sia-menubar-menu>
 * <a-sia-menubar-menu label="LABEL NAME 2">
 *   <a-sia-menubar-entry [action]="ONCLICK CALLBACK FOR ENTRY">ENTRY NAME 1</a-sia-menubar-entry>
 * </a-sia-menubar-menu>
 * ```
 */
@Component({
  selector: 'a-sia-menubar',
  templateUrl: './menubar.component.html',
  styleUrls: ['./menubar.component.scss']
})
export class MenubarComponent {
  constructor(
    private inputService: InputService,
    private menubarService: MenubarService,
    public paneService: PaneService,
    public modalService: ModalService
  ) {
    this.inputService.onMouseDownSubject.subscribe((event) => this.closeWhenMenubarNotClicked(event));
  }

  /**
   * Get all MenubarMenus.
   * {read: ElementRef} is needed to get the Element instead of the Component
   */
  @ViewChildren(MenubarMenuComponent, { read: ElementRef }) menubarMenus!: QueryList<ElementRef>;

  public menubarExampleCallback(): void {
    console.log('example menubar callback was invoked');
  }

  /**
   * checks if some of the menus contain the event.target, if not it closes all menus
   */
  closeWhenMenubarNotClicked(event: MouseEvent): void {
    if (!this.menubarMenus.some((menu) => menu.nativeElement.contains(event.target))) {
      this.menubarService.closeAll();
    }
  }

  openLoginModal = (): void => {
    this.modalService.openModal('login');
  };
}
