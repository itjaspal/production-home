import { CommonService } from './_service/common.service';
import { DropdownlistService } from './_service/dropdownlist.service';
import { NgModule, ApplicationRef } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import {
  MatAutocompleteModule,
  MatButtonModule,
  MatButtonToggleModule,
  MatCardModule,
  MatCheckboxModule,
  MatChipsModule,
  MatDatepickerModule,
  MatDialogModule,
  MatExpansionModule,
  MatGridListModule,
  MatIconModule,
  MatInputModule,
  MatListModule,
  MatMenuModule,
  MatNativeDateModule,
  MatPaginatorModule,
  MatProgressBarModule,
  MatProgressSpinnerModule,
  MatRadioModule,
  MatRippleModule,
  MatSelectModule,
  MatSidenavModule,
  MatSliderModule,
  MatSlideToggleModule,
  MatSnackBarModule,
  MatSortModule,
  MatTableModule,
  MatTabsModule,
  MatToolbarModule,
  MatTooltipModule,
  MatStepperModule,
  MAT_DATE_FORMATS,
  DateAdapter
} from '@angular/material';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
// Layout
import { LayoutComponent } from './layout/layout.component';
import { PreloaderDirective } from './layout/preloader.directive';
// Header
import { AppHeaderComponent } from './layout/header/header.component';
// Sidenav
import { AppSidenavComponent } from './layout/sidenav/sidenav.component';
import { ToggleOffcanvasNavDirective } from './layout/sidenav/toggle-offcanvas-nav.directive';
import { AutoCloseMobileNavDirective } from './layout/sidenav/auto-close-mobile-nav.directive';
import { AppSidenavMenuComponent } from './layout/sidenav/sidenav-menu/sidenav-menu.component';
import { AccordionNavDirective } from './layout/sidenav/sidenav-menu/accordion-nav.directive';
import { AppendSubmenuIconDirective } from './layout/sidenav/sidenav-menu/append-submenu-icon.directive';
import { HighlightActiveItemsDirective } from './layout/sidenav/sidenav-menu/highlight-active-items.directive';
// Customizer
import { AppCustomizerComponent } from './layout/customizer/customizer.component';
import { ToggleQuickviewDirective } from './layout/customizer/toggle-quickview.directive';
// Footer
import { AppFooterComponent } from './layout/footer/footer.component';
// Search Overaly
import { AppSearchOverlayComponent } from './layout/search-overlay/search-overlay.component';
import { SearchOverlayDirective } from './layout/search-overlay/search-overlay.directive';
import { OpenSearchOverlaylDirective } from './layout/search-overlay/open-search-overlay.directive';

// Sub modules
import { LayoutModule } from './layout/layout.module';
import { SharedModule } from './shared/shared.module';

//3rd party
import { NgxMatSelectSearchModule } from 'ngx-mat-select-search';
import { ZXingScannerModule } from '@zxing/ngx-scanner';
import { SignaturePadModule } from 'angular2-signaturepad';

// hmr
import { removeNgStyles, createNewHosts } from '@angularclass/hmr';
import { AppMobileFooterComponent } from './layout/mobile-footer/mobile-footer.component';
import { AppMobileHeaderComponent } from './layout/mobile-header/mobile-header.component';

//http interceptor
import { HTTP_INTERCEPTORS, HttpClient, HttpClientModule } from '@angular/common/http';
import { HttpModule } from '@angular/http';
import { TokenInterceptor } from './_service/token.interceptor';

import { CustomDateAdapter, APP_DATE_FORMATS } from './_common/custom-date-adapter';

// Pages

import { MobileMenuComponent } from './mobile-menu/mobile-menu.component';
import { UserLoginComponent } from './user-login/user-login.component';
import { BranchGroupSearchComponent } from './master-branch-group/branch-group-search/branch-group-search.component';
import { BranchGroupCreateComponent } from './master-branch-group/branch-group-create/branch-group-create.component';
import { BranchGroupUpdateComponent } from './master-branch-group/branch-group-update/branch-group-update.component';
import { BranchGroupViewComponent } from './master-branch-group/branch-group-view/branch-group-view.component';
import { DepartmentSearchComponent } from './master-department/department-search/department-search.component';
import { DepartmentCreateComponent } from './master-department/department-create/department-create.component';
import { DepartmentUpdateComponent } from './master-department/department-update/department-update.component';
import { DepartmentViewComponent } from './master-department/department-view/department-view.component';

//service

import { LoaderService } from './_service/loader.service';
import { BranchSearchComponent } from './master-branch/branch-search/branch-search.component';
import { BranchSaveComponent } from './master-branch/branch-save/branch-save.component';
import { BranchViewComponent } from './master-branch/branch-view/branch-view.component';
import { MobileProfileComponent } from './mobile-profile/mobile-profile.component';
import { InqUserComponent } from './master-user/inq/inq-user.component';
import { UserService } from './_service/user.service';
import { CreateUserComponent } from './master-user/create/create-user.component';
import { PopupMessageComponent } from './modal/message/popup-message.component';
import { MessageService } from './_service/message.service';
import { ViewUserComponent } from './master-user/view/view-user.component';
import { UpdateUserComponent } from './master-user/update/update-user.component';
import { DisableControlDirective } from './_directive/disable-control.component';
import { ResetPasswordUserComponent } from './master-user/reset-password/reset-user.component';
import { UserRoleService } from './_service/user-role.service';
import { InqUserRoleComponent } from './master-user-role/inq/inq-user-role.component';
import { CreateUserRoleComponent } from './master-user-role/create/create-user-role.component';
import { ViewUserRoleComponent } from './master-user-role/view/view-user-role.component';
import { UpdateUserRoleComponent } from './master-user-role/update/update-user-role.component';

import { ChangePasswordComponent } from './master-user/change-password/change-password.component';
import { HomeComponent } from './home/home.component';

import { ListFilterPipe } from './_pipe/list-filter.pipe';
import { HighlightPipe } from './_pipe/highlight.pipe';

import { ConfirmMessageComponent } from './modal/confirm-message/confirm-message.component';

import { BuilderFilterPipe } from './_pipe/array-filter.pipe';

import { MenuGroupCreateComponent } from './master-menu-group/menu-group-create/menu-group-create.component';
import { MenuGroupSearchComponent } from './master-menu-group/menu-group-search/menu-group-search.component';
import { MenuGroupUpdateComponent } from './master-menu-group/menu-group-update/menu-group-update.component';
import { MenuGroupViewComponent } from './master-menu-group/menu-group-view/menu-group-view.component';
import { MenuGroupService } from './_service/menu-group.service';
import { MenuCreateComponent } from './master-menu/menu-create/menu-create.component';
import { MenuSearchComponent } from './master-menu/menu-search/menu-search.component';
import { MenuUpdateComponent } from './master-menu/menu-update/menu-update.component';
import { MenuViewComponent } from './master-menu/menu-view/menu-view.component';
import { MenuService } from './_service/menu.service';
import { BranchSearchAssignProductComponent } from './master-branch/branch-search-assignProduct/branch-search-assignProduct.component';
import { UserSelectBranchComponent } from './user-select-branch/user-select-branch/user-select-branch.component';
import { DefaultPrinterComponent } from './default-printer/default-printer/default-printer.component';
import { SpecDrawingComponent } from './spec-drawing/spec-drawing/spec-drawing.component';
import { InprocessScanAddComponent } from './scan-inprocess/inprocess-scan-add/inprocess-scan-add.component';
import { InprocessScanCancelComponent } from './scan-inprocess/inprocess-scan-cancel/inprocess-scan-cancel.component';
import { InprocessEntryAddComponent } from './scan-inprocess/inprocess-entry-add/inprocess-entry-add.component';
import { InprocessEntryCancelComponent } from './scan-inprocess/inprocess-entry-cancel/inprocess-entry-cancel.component';
import { InprocessSearchComponent } from './scan-inprocess/inprocess-search/inprocess-search.component';
import { JobOperationComponent } from './job-operation/job-operation/job-operation.component';
import { JobOrderDetailComponent } from './job-operation/job-order-detail/job-order-detail.component';
import { InprocessComponent } from './scan-inprocess/inprocess/inprocess.component';
import { ProductSearchComponent } from './scan-inprocess/product-search/product-search.component';
import { ProductCancelSearchComponent } from './scan-inprocess/product-cancel-search/product-cancel-search.component';
import { PrintInprocessComponent } from './print-inprocess-tag/print-inprocess/print-inprocess.component';
import { PrintInprocessDetailComponent } from './print-inprocess-tag/print-inprocess-detail/print-inprocess-detail.component';
import { PrintInprocessTagComponent } from './print-inprocess-tag/print-inprocess-tag/print-inprocess-tag.component';
import { InprocessTagProductSearchComponent } from './print-inprocess-tag/inprocess-tag-product-search/inprocess-tag-product-search.component';
import { JobOrderSummaryComponent } from './job-operation/job-order-summary/job-order-summary.component';
import { ScanSendComponent } from './scan-send/scan-send/scan-send.component';
import { ScanSendSetnoSearchComponent } from './scan-send/scan-send-setno-search/scan-send-setno-search.component';
import { ViewSpecDrawingComponent } from './job-operation/view-spec-drawing/view-spec-drawing.component';
import { ProductionRecListComponent } from './production-rec-report/production-rec-list/production-rec-list.component';
import { ProductionRecListDetailComponent } from './production-rec-report/production-rec-list-detail/production-rec-list-detail.component';
import { ScanApproveSendSearchComponent } from './scan-approve-send/scan-approve-send-search/scan-approve-send-search.component';
import { ScanApproveSendCreateComponent } from './scan-approve-send/scan-approve-send-create/scan-approve-send-create.component';
import { ScanApproveSendAddComponent } from './scan-approve-send/scan-approve-send-add/scan-approve-send-add.component';
import { ScanApproveSendCancelComponent } from './scan-approve-send/scan-approve-send-cancel/scan-approve-send-cancel.component';
import { ScanApproveSendDetailComponent } from './scan-approve-send/scan-approve-send-detail/scan-approve-send-detail.component';
import { ScanReceiveSearchComponent } from './scan-receive/scan-receive-search/scan-receive-search.component';
import { ScanReceiveCreateComponent } from './scan-receive/scan-receive-create/scan-receive-create.component';
import { ScanReceiveProductViewComponent } from './scan-receive/scan-receive-product-view/scan-receive-product-view.component';
import { ProductDefectSearchComponent } from './product-defect/product-defect-search/product-defect-search.component';
import { ProductDefectQcEntryComponent } from './product-defect/product-defect-qc-entry/product-defect-qc-entry.component';
import { ProductDefectQcCuttingComponent } from './product-defect/product-defect-qc-cutting/product-defect-qc-cutting.component';
import { ItemSearchComponent } from './product-defect/item-search/item-search.component';
import { ItemEntrySearchComponent } from './product-defect/item-entry-search/item-entry-search.component';
import { OrderDetailComponent } from './product-defect/order-detail/order-detail.component';
import { ViewSpecComponent } from './product-defect/view-spec/view-spec.component';
import { ScanPutawayComponent } from './scan_putaway/scan-putaway/scan-putaway.component';
import { ScanPutawayBarcodeComponent } from './scan_putaway/scan-putaway-barcode/scan-putaway-barcode.component';
import { ScanPutawayDetailComponent } from './scan_putaway/scan-putaway-detail/scan-putaway-detail.component';
import { ScanPutawayQrComponent } from './scan_putaway/scan-putaway-qr/scan-putaway-qr.component';
import { JobOperationStockComponent } from './job-operation/job-operation-stock/job-operation-stock.component';
import { PorDetailComponent } from './job-operation/por-detail/por-detail.component';
import { ProductDetailComponent } from './job-operation/product-detail/product-detail.component';
import { ScanInprocessStockComponent } from './scan-inprocess-stock/scan-inprocess-stock/scan-inprocess-stock.component';
import { ScanInprocessStockScanAddComponent } from './scan-inprocess-stock/scan-inprocess-stock-scan-add/scan-inprocess-stock-scan-add.component';
import { ScanInprocessStockEntryAddComponent } from './scan-inprocess-stock/scan-inprocess-stock-entry-add/scan-inprocess-stock-entry-add.component';
import { ScanInprocessStockCancelComponent } from './scan-inprocess-stock/scan-inprocess-stock-cancel/scan-inprocess-stock-cancel.component';
import { SubProductSearchComponent } from './scan-inprocess-stock/sub-product-search/sub-product-search.component';
import { SubProductCancelSearchComponent } from './scan-inprocess-stock/sub-product-cancel-search/sub-product-cancel-search.component';
import { PrintInprocessStockComponent } from './print-inprocess-tag-stock/print-inprocess-stock/print-inprocess-stock.component';
import { PrintInprocessTagStockComponent } from './print-inprocess-tag-stock/print-inprocess-tag-stock/print-inprocess-tag-stock.component';
import { InprocessTagStockProductSearchComponent } from './print-inprocess-tag-stock/inprocess-tag-stock-product-search/inprocess-tag-stock-product-search.component';
import { InprocessTagStockGroupSearchComponent } from './print-inprocess-tag-stock/inprocess-tag-stock-group-search/inprocess-tag-stock-group-search.component';
import { ScanDefectComponent } from './scan-defect/scan-defect/scan-defect.component';
import { ScanDefectEntryAddComponent } from './scan-defect/scan-defect-entry-add/scan-defect-entry-add.component';
import { ScanDefectEntryCancelComponent } from './scan-defect/scan-defect-entry-cancel/scan-defect-entry-cancel.component';
import { SubProductDefectSearchComponent } from './scan-defect/sub-product-defect-search/sub-product-defect-search.component';
import { ScanDefectRemarkComponent } from './scan-defect/scan-defect-remark/scan-defect-remark.component';
import { ScanDefectScanAddComponent } from './scan-defect/scan-defect-scan-add/scan-defect-scan-add.component';
import { SubProductDefectCancelSearchComponent } from './scan-defect/sub-product-defect-cancel-search/sub-product-defect-cancel-search.component';
import { SummaryDefectComponent } from './scan-defect/summary-defect/summary-defect.component';
import { ProductGroupDetailComponent } from './job-operation/product-group-detail/product-group-detail.component';
import { ScanPutawayProductViewComponent } from './scan_putaway/scan-putaway-product-view/scan-putaway-product-view.component';
import { UploadFileSearchComponent } from './upload-file/upload-file-search/upload-file-search.component';
import { UploadFileAddComponent } from './upload-file/upload-file-add/upload-file-add.component';
import { UploadFileEditComponent } from './upload-file/upload-file-edit/upload-file-edit.component';
import { ProductionTrackingComponent } from './job-operation/production-tracking/production-tracking.component';
import { ProductionTrackingStockComponent } from './job-operation/production-tracking-stock/production-tracking-stock.component';
import { ScanCheckQrComponent } from './scan-receive/scan-check-qr/scan-check-qr.component';
import { OrderSearchComponent } from './scan-inprocess-stock/order-search/order-search.component';
import { OrderAllSearchComponent } from './job-operation/order-all-search/order-all-search.component';
import { ProductDefectOrderSearchComponent } from './product-defect/product-defect-order-search/product-defect-order-search.component';
import { ScanDefectOrderSearchComponent } from './scan-defect/scan-defect-order-search/scan-defect-order-search.component';
import { ProductionTrackingGroupComponent } from './job-operation/production-tracking-group/production-tracking-group.component';
import { ScanPutawayCheckQrComponent } from './scan_putaway/scan-putaway-check-qr/scan-putaway-check-qr.component';





@NgModule({
  imports: [
    BrowserModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule.withConfig({ warnOnNgModelWithFormControl: 'never' }),
    BrowserAnimationsModule,
    MatAutocompleteModule,
    MatButtonModule,
    MatButtonToggleModule,
    MatCardModule,
    MatCheckboxModule,
    MatChipsModule,
    MatStepperModule,
    MatDatepickerModule,
    MatDialogModule,
    MatExpansionModule,
    MatGridListModule,
    MatIconModule,
    MatInputModule,
    MatListModule,
    MatMenuModule,
    MatNativeDateModule,
    MatPaginatorModule,
    MatProgressBarModule,
    MatProgressSpinnerModule,
    MatRadioModule,
    MatRippleModule,
    MatSelectModule,
    MatSidenavModule,
    MatSliderModule,
    MatSlideToggleModule,
    MatSnackBarModule,
    MatSortModule,
    MatTableModule,
    MatTabsModule,
    MatToolbarModule,
    MatTooltipModule,
    AppRoutingModule,
    NgbModule,

    // Sub modules
    LayoutModule,
    SharedModule,

    //3rd party
    NgxMatSelectSearchModule,
    ZXingScannerModule.forRoot(),
    SignaturePadModule,
  ],
  providers: [
    // AuthGuard,
    LoaderService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: TokenInterceptor,
      multi: true
    },
    {
      provide: DateAdapter, useClass: CustomDateAdapter
    },
    {
      provide: MAT_DATE_FORMATS, useValue: APP_DATE_FORMATS
    },


    //OrganizationService,
    DropdownlistService,
    UserService,
    UserRoleService,
    MessageService,
    CommonService,
   
    MenuGroupService,
    MenuService,
    

  ],
  declarations: [
    DisableControlDirective,
    AppComponent,
    // Layout
    LayoutComponent,
    PreloaderDirective,
    // Header
    AppHeaderComponent,
    AppMobileHeaderComponent,
    // Sidenav
    AppSidenavComponent,
    ToggleOffcanvasNavDirective,
    AutoCloseMobileNavDirective,
    AppSidenavMenuComponent,
    AccordionNavDirective,
    AppendSubmenuIconDirective,
    HighlightActiveItemsDirective,
    // Customizer
    AppCustomizerComponent,
    ToggleQuickviewDirective,
    // Footer
    AppFooterComponent,
    AppMobileFooterComponent,
    // Search overlay
    AppSearchOverlayComponent,
    SearchOverlayDirective,
    OpenSearchOverlaylDirective,
    // pipe
    ListFilterPipe,
    //
    
    MobileMenuComponent,
    MobileProfileComponent,
    UserLoginComponent,
    BranchGroupSearchComponent,
    BranchGroupCreateComponent,
    BranchGroupUpdateComponent,
    BranchGroupViewComponent,
    BranchSearchAssignProductComponent,
    DepartmentSearchComponent,
    DepartmentCreateComponent,
    DepartmentUpdateComponent,
    DepartmentViewComponent,
    BranchSearchComponent,
    BranchSaveComponent,
    BranchViewComponent,

    //User Component
    InqUserComponent,
    CreateUserComponent,
    ViewUserComponent,
    UpdateUserComponent,
    ResetPasswordUserComponent,
    ChangePasswordComponent,

    //User Role Component
    InqUserRoleComponent,
    CreateUserRoleComponent,
    ViewUserRoleComponent,
    UpdateUserRoleComponent,

    
    PopupMessageComponent,
    ConfirmMessageComponent,
    
    HomeComponent,

    
    ListFilterPipe,

    
    HighlightPipe,

    

    BuilderFilterPipe,
    
    
    MenuGroupCreateComponent,
    MenuGroupSearchComponent,
    MenuGroupUpdateComponent,
    MenuGroupViewComponent,
    MenuCreateComponent,
    MenuSearchComponent,
    MenuUpdateComponent,
    MenuViewComponent,
    UserSelectBranchComponent,
    DefaultPrinterComponent,
    SpecDrawingComponent,
    InprocessScanAddComponent,
    InprocessScanCancelComponent,
    InprocessEntryAddComponent,
    InprocessEntryCancelComponent,
    InprocessSearchComponent,
    JobOperationComponent,
    JobOrderDetailComponent,
    InprocessComponent,
    ProductSearchComponent,
    ProductCancelSearchComponent,
    PrintInprocessComponent,
    PrintInprocessDetailComponent,
    PrintInprocessTagComponent,
    InprocessTagProductSearchComponent,
    JobOrderSummaryComponent,
    ScanSendComponent,
    ScanSendSetnoSearchComponent,
    ViewSpecDrawingComponent,
    ProductionRecListComponent,
    ProductionRecListDetailComponent,
    ScanApproveSendSearchComponent,
    ScanApproveSendCreateComponent,
    ScanApproveSendAddComponent,
    ScanApproveSendCancelComponent,
    ScanApproveSendDetailComponent,
    ScanReceiveSearchComponent,
    ScanReceiveCreateComponent,
    ScanReceiveProductViewComponent,
    ProductDefectSearchComponent,
    ProductDefectQcEntryComponent,
    ProductDefectQcCuttingComponent,
    ItemSearchComponent,
    ItemEntrySearchComponent,
    OrderDetailComponent,
    ViewSpecComponent,
    ScanPutawayComponent,
    ScanPutawayBarcodeComponent,
    ScanPutawayDetailComponent,
    ScanPutawayQrComponent,
    JobOperationStockComponent,
    PorDetailComponent,
    ProductDetailComponent,
    ScanInprocessStockComponent,
    ScanInprocessStockScanAddComponent,
    ScanInprocessStockEntryAddComponent,
    ScanInprocessStockCancelComponent,
    SubProductSearchComponent,
    SubProductCancelSearchComponent,
    PrintInprocessStockComponent,
    PrintInprocessTagStockComponent,
    InprocessTagStockProductSearchComponent,
    InprocessTagStockGroupSearchComponent,
    ScanDefectComponent,
    ScanDefectEntryAddComponent,
    ScanDefectEntryCancelComponent,
    SubProductDefectSearchComponent,
    ScanDefectRemarkComponent,
    ScanDefectScanAddComponent,
    SubProductDefectCancelSearchComponent,
    SummaryDefectComponent,
    ProductGroupDetailComponent,
    ScanPutawayProductViewComponent,
    UploadFileSearchComponent,
    UploadFileAddComponent,
    UploadFileEditComponent,
    ProductionTrackingComponent,
    ProductionTrackingStockComponent,
    ScanCheckQrComponent,
    OrderSearchComponent,
    OrderAllSearchComponent,
    ProductDefectOrderSearchComponent,
    ScanDefectOrderSearchComponent,
    ProductionTrackingGroupComponent,
    ScanPutawayCheckQrComponent,
    

  
  
    
  ],
  entryComponents: [
    PopupMessageComponent,
    ConfirmMessageComponent,
    ProductSearchComponent,
    ProductCancelSearchComponent,
    PrintInprocessTagComponent,
    PrintInprocessDetailComponent,
    InprocessTagProductSearchComponent,
    ScanSendSetnoSearchComponent,
    ScanApproveSendCreateComponent,
    ScanReceiveCreateComponent,
    ScanReceiveProductViewComponent,
    ScanApproveSendCancelComponent,
    ScanApproveSendAddComponent,
    ScanApproveSendDetailComponent,
    ProductDefectQcCuttingComponent,
    ProductDefectQcEntryComponent,
    ItemSearchComponent,
    ItemEntrySearchComponent,
    OrderDetailComponent,
    ViewSpecComponent,
    PorDetailComponent,
    ProductDetailComponent,
    ScanInprocessStockScanAddComponent,
    ScanInprocessStockEntryAddComponent,
    SubProductSearchComponent,
    ScanInprocessStockCancelComponent,
    SubProductCancelSearchComponent,
    PrintInprocessTagStockComponent,
    InprocessTagStockProductSearchComponent,
    InprocessTagStockGroupSearchComponent,
    ScanDefectScanAddComponent,
    SubProductDefectSearchComponent,
    ScanDefectEntryAddComponent,
    ScanDefectRemarkComponent,
    ScanDefectEntryCancelComponent,
    SubProductDefectCancelSearchComponent,
    ScanDefectRemarkComponent,
    SummaryDefectComponent,
    ProductGroupDetailComponent,
    ProductionTrackingComponent,
    ProductionTrackingStockComponent,
    ScanCheckQrComponent,
    OrderSearchComponent,
    OrderAllSearchComponent,
    ScanDefectOrderSearchComponent,
    ProductDefectOrderSearchComponent,
    ProductionTrackingGroupComponent
  ],
  bootstrap: [AppComponent]
})

export class AppModule {
  constructor(public appRef: ApplicationRef) { }
  hmrOnInit(store) {
    console.log('HMR store', store);
  }
  hmrOnDestroy(store) {
    const cmpLocation = this.appRef.components.map((cmp) => cmp.location.nativeElement);
    // recreate elements
    store.disposeOldHosts = createNewHosts(cmpLocation);
    // remove styles
    removeNgStyles();
  }
  hmrAfterDestroy(store) {
    // display new elements
    store.disposeOldHosts();
    delete store.disposeOldHosts;
  }
}
