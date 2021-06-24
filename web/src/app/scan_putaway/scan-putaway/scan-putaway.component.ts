import { AppSetting } from './../../_constants/app-setting';
import { ScanPutawayProductViewComponent } from './../scan-putaway-product-view/scan-putaway-product-view.component';
import { ProductionRecListDetailComponent } from './../../production-rec-report/production-rec-list-detail/production-rec-list-detail.component';
import { Component, OnInit, ViewChild, ElementRef, Inject } from '@angular/core';
import { PageEvent, MatDialogRef, MatDialog } from '@angular/material';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthenticationService } from '../../_service/authentication.service';
import { DatePipe } from '@angular/common'
import { AbstractControl, FormBuilder, FormGroup, Validators } from '@angular/forms';
import * as moment from 'moment';
import { productionRecListDetailView, productionRecListSearchView, productionRecListTotalView } from '../../_model/production-rec-list';
import { ProductionRecListService } from '../../_service/production-rec-list.service';
import { ScanPutawayCheckQrComponent } from '../scan-putaway-check-qr/scan-putaway-check-qr.component';

@Component({
  selector: 'app-scan-putaway',
  templateUrl: './scan-putaway.component.html',
  styleUrls: ['./scan-putaway.component.scss']
})
export class ScanPutawayComponent implements OnInit {

  public model: productionRecListSearchView = new productionRecListSearchView();
  public datas: productionRecListTotalView<productionRecListDetailView> = new productionRecListTotalView<productionRecListDetailView>();

  public user: any; 
  public datePipe = new DatePipe('en-US'); 
  public validationForm: FormGroup;
  public vEntity: any = AppSetting.entity; 

  @ViewChild('doc_date') doc_date: ElementRef;
  @ViewChild('doc_status') doc_status: ElementRef;
  @ViewChild('doc_no') doc_no: ElementRef;

  constructor(
    private _productionRecListSvc: ProductionRecListService,
    private _authSvc: AuthenticationService,
    private _activateRoute: ActivatedRoute,
    private _router: Router,
    private _formBuilder: FormBuilder,
    private _dialog: MatDialog,
  ) { }

  ngOnInit() {
    this.buildForm();
    this.user = this._authSvc.getLoginUser(); 

    //console.log(sessionStorage.getItem('S2-doc_no') + "|" + sessionStorage.getItem('S2-doc_date') + "|" + sessionStorage.getItem('S2-doc_status'));

    this.doc_no.nativeElement.value     = sessionStorage.getItem('S2-doc_no');
    this.model.doc_status  = sessionStorage.getItem('S2-doc_status');

    if (sessionStorage.getItem('Exit_Status') == "" || sessionStorage.getItem('Exit_Status') == null ) {
       this.doc_date.nativeElement.value   =  this.datePipe.transform(new Date(),"dd/MM/yyyy");
    } else { 
       this.doc_date.nativeElement.value   = sessionStorage.getItem('S2-doc_date');
    }


    //this.doc_no.nativeElement.value = sessionStorage.getItem('Session-doc_no');
    if (this.model.doc_status == "" || this.model.doc_status == null){this.model.doc_status = 'PAL';}
    //this.model.doc_status = 'PAL';
    this.searchProductionRecList();
  }

  buildForm() {
    this.validationForm = this._formBuilder.group({
      doc_date: ['', Validators.compose([Validators.required, YourValidator.dateVaidator])],
      doc_no: [null, []],
      doc_status: [null, []]
    });
  }

  close() { 
    this._router.navigateByUrl('/app/mobile-navigator'); 
  }

  set_session(){
    sessionStorage.setItem('S1-doc_no', this.model.doc_no);
    sessionStorage.setItem('S1-doc_date', this.model.doc_date);
    sessionStorage.setItem('S1-doc_status', this.model.doc_status);
  }
  
  ngOnDestroy() {
    //console.log("Close Program ");
    sessionStorage.removeItem('S2-doc_no');
    sessionStorage.removeItem('S2-doc_date');
    sessionStorage.removeItem('S2-doc_status');
    sessionStorage.removeItem('Exit_Status');
  }

  async searchProductionRecList(event: PageEvent = null) {   

      if (event != null) {
        this.model.pageIndex = event.pageIndex;
        this.model.itemPerPage = event.pageSize;
      }
      
      this.datas.recDetails  = [];
      this.model.build_type   = this.user.branch.entity_code;
      this.model.doc_date   = this.doc_date.nativeElement.value; // 16/10/2020
    
      this.model.doc_no       = this.doc_no.nativeElement.value;
      //this.model.doc_status   = this.doc_status.nativeElement.value;
      console.log(this.model.doc_date);
      //this.model.doc_date     = "";

      this.datas =  await this._productionRecListSvc.searchPutAwayWaiting(this.model);
      console.log(this.datas);
      this.doc_date.nativeElement.value = this.model.doc_date;
     
  }

  openProductDetail(p_entity : string ,p_doc_no: string  , _index: number = -1)
  {
    const dialogRef = this._dialog.open(ScanPutawayProductViewComponent, {
      maxWidth: '100vw',
      maxHeight: '100vh',
      height: '100%',
      width: '100%',
      data: {
        doc_no: p_doc_no,
        entity:p_entity
       
      }

    });

    // dialogRef.afterClosed().subscribe(result => {
    //   if (result.length > 0) {
    //     //this.add_prod(result);
    //   }
    // })
  }


  openScanCheckQr()
  {
      const dialogRef = this._dialog.open(ScanPutawayCheckQrComponent, {
      maxWidth: '100vw',
      maxHeight: '100vh', 
      height: '100%',
      width: '100%',
      data: {
              
      }

    });
    /*dialogRef.afterClosed().subscribe(result => {
      this.search();
      // if (result.length > 0) {
      //   //this.add_prod(result);
      // }
    }) */
  } 


 /* openProductionRecListDetailDialog(p_docNo: string, p_docDate: string, _isEdit: boolean = false, _index: number = -1) {
    const dialogRef = this._dialog.open(ProductionRecListDetailComponent, {
      maxWidth: '100vw',
      maxHeight: '100vh', 
      height: '100%',
      width: '100%',
      data: {
        doc_no: p_docNo,
        doc_date: p_docDate,
        isEdit: _isEdit,
       // editItem: _editItem,
        hideSerialNo: true,
        isSaleBed: false
      }
    });
  
    dialogRef.afterClosed().subscribe(result => { 
      if (result) {
        
      }
    });
  }*/





}


export class YourValidator {
  static dateVaidator(AC: AbstractControl) {

    if (AC && AC.value && !moment(AC.value, 'YYYY-MM-DD',true).isValid()) {
      return {'dateVaidator': true};
    }
    return null;  
  }
}