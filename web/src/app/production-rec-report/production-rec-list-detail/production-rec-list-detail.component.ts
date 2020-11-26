import { Component, OnInit, ViewChild, ElementRef, Inject, ChangeDetectorRef } from '@angular/core';
import { PageEvent, MatDialogRef, MatDialog, MAT_DIALOG_DATA } from '@angular/material';
import { ActivatedRoute, Router } from '@angular/router';
import { DatePipe } from '@angular/common'
import { AbstractControl, FormBuilder, FormGroup, Validators } from '@angular/forms';
import * as moment from 'moment';
import { productionRecListDetailSearchView, productionRecListDetailTotalView, productionRecListDetailProdView, productionRecListParamView } from '../../_model/production-rec-list';
import { AuthenticationService } from '../../_service/authentication.service';
import { ProductionRecListService } from '../../_service/production-rec-list.service';
import { SendProductSearch } from '../../_model/scan-receive';
import { DropdownlistService } from '../../_service/dropdownlist.service';
import { MessageService } from '../../_service/message.service';
import { ScanReceiveService } from '../../_service/scan-receive.service';

@Component({
  selector: 'app-production-rec-list-detail',
  templateUrl: './production-rec-list-detail.component.html',
  styleUrls: ['./production-rec-list-detail.component.scss']
})
export class ProductionRecListDetailComponent implements OnInit {

  constructor(
    public dialogRef: MatDialogRef<any>,
    @Inject(MAT_DIALOG_DATA) public data: SendProductSearch,
    private _fb: FormBuilder,
    private _authSvc: AuthenticationService,
    private _dialog: MatDialog,
    private _msgSvc: MessageService,
    private _router: Router,
    private _actRoute: ActivatedRoute,
    private _scanRecSvc: ScanReceiveService,
    private cdr: ChangeDetectorRef,
    private _dll: DropdownlistService,) { }


  public datas: any = {};

  async ngOnInit() {
    console.log(this.data.doc_no);
    console.log(this.data.entity);

    this.datas =  await this._scanRecSvc.getProductDetail(this.data.entity,this.data.doc_no);

    console.log(this.datas);
  }

  close() { 
    this.dialogRef.close();
  }
//   public model: productionRecListDetailSearchView = new productionRecListDetailSearchView();
//   public datas: productionRecListDetailTotalView<productionRecListDetailProdView> = new productionRecListDetailTotalView<productionRecListDetailProdView>();

//   public user: any; 
//   public datePipe = new DatePipe('en-US'); 
//   public validationForm: FormGroup;

//   constructor(
//     private _productionRecListSvc: ProductionRecListService,
//     private _authSvc: AuthenticationService,
//     private _activateRoute: ActivatedRoute,
//     private _router: Router,
//     private _formBuilder: FormBuilder,
//     public dialogRef: MatDialogRef<any>,
//     private _dialog: MatDialog,
//     @Inject(MAT_DIALOG_DATA) public paramData: productionRecListParamView
//   ) { }

//   ngOnInit() {
//     this.user = this._authSvc.getLoginUser();
//     this.searchProductionRecListDetail();
//   }

//   close() { 
    
//     this.dialogRef.close();
//   }

//   ngOnDestroy() {
   
//   }

//   async searchProductionRecListDetail(event: PageEvent = null) {   

//     if (event != null) {
//       this.model.pageIndex = event.pageIndex;
//       this.model.itemPerPage = event.pageSize;
//     }
    
//     this.datas.recDetails  = [];
//     this.model.build_type   = this.user.branch.entity_code;

//     console.log("paramData.doc_no : " + this.paramData.doc_no);
//     console.log("paramData.doc_date : " + this.paramData.doc_date);

//     this.model.doc_date     = this.datePipe.transform(this.paramData.doc_date, 'dd/MM/yyyy')//"16/10/2020";
//     this.model.doc_no       = this.paramData.doc_no//"PH20100002";
    

//     this.datas =  await this._productionRecListSvc.searchProductionRecListDetail(this.model);
//     console.log(this.datas);
//     //this.doc_date.nativeElement.value = this.model.doc_date;
   
// }



}
