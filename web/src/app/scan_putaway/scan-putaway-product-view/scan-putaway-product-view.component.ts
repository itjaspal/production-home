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
  selector: 'app-scan-putaway-product-view',
  templateUrl: './scan-putaway-product-view.component.html',
  styleUrls: ['./scan-putaway-product-view.component.scss']
})
export class ScanPutawayProductViewComponent implements OnInit {

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




}