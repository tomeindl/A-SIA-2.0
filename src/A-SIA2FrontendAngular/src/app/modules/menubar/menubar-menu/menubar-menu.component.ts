import { Component, HostBinding, Input } from '@angular/core';
import { MenubarService } from '../menubar.service';

/**
 * A Menubar menu, which contains multiple entries.
 * The opening and closing of the menu is implemented on hover of the menubar-menu component
 * @example
 * for example, ng-content will be replaced with:
 * ```html
 *   <a-sia-menubar-entry [action]="ONCLICK CALLBACK FOR ENTRY">ENTRY NAME 1</a-sia-menubar-entry>
 * ```
 */
@Component({
  selector: 'a-sia-menubar-menu',
  templateUrl: './menubar-menu.component.html',
  styleUrls: ['./menubar-menu.component.scss']
})
export class MenubarMenuComponent {
  /**
   * The text to be displayed in the main menubar. e.g. File, View, Edit
   */
  @Input() label = 'Menu';

  /**
   * tracks current open / close status, used for toggle
   */
  @HostBinding('class.open') isOpen = false;

  /**
   * adds this menu to the list of all menu, i.e. to be able to close all menus
   * @param menubarService service overseeing the whole menubar
   */
  constructor(private menubarService: MenubarService) {
    this.menubarService.addMenu(this);
  }

  /**
   * Opens the menu on mouseEnter if the menubar was clicked
   */
  onLabelMouseEnter(): void {
    if (this.menubarService.menuWasClicked) this.openMenu();
  }

  /**
   * Opens the menu
   */
  onLabelClick(): void {
    this.toggleMenu();
    this.menubarService.menuWasClicked = !this.menubarService.menuWasClicked;
  }

  /**
   * calls closeMenu() or openMenu()
   */
  toggleMenu(): void {
    if (this.isOpen) this.closeMenu();
    else this.openMenu();
  }

  openMenu(): void {
    this.menubarService.closeMenusExcept(this);
    this.isOpen = true;
  }

  closeMenu(): void {
    this.isOpen = false;
  }
}
