import { DatePipe } from '@angular/common';
import { ChangeDetectorRef, Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog, PageEvent } from '@angular/material';
import { Router, ActivatedRoute } from '@angular/router';
import { ScanApproveSendCreateComponent } from '../../scan-approve-send/scan-approve-send-create/scan-approve-send-create.component';
import { ScanReceiveDataView, ScanReceiveDataSearchView } from '../../_model/scan-receive';
import { AuthenticationService } from '../../_service/authentication.service';
import { DropdownlistService } from '../../_service/dropdownlist.service';
import { MessageService } from '../../_service/message.service';
import { ScanReceiveService } from '../../_service/scan-receive.service';
import { ScanReceiveCreateComponent } from '../scan-receive-create/scan-receive-create.component';
import { ScanReceiveProductViewComponent } from '../scan-receive-product-view/scan-receive-product-view.component';

@Component({
  selector: 'app-scan-receive-search',
  templateUrl: './scan-receive-search.component.html',
  styleUrls: ['./scan-receive-search.component.scss']
})
export class ScanReceiveSearchComponent implements OnInit {

  @ViewChild('doc_date') doc_date: ElementRef;

  constructor(
    private _fb: FormBuilder,
    private _authSvc: AuthenticationService,
    private _dialog: MatDialog,
    private _msgSvc: MessageService,
    private _router: Router,
    private _actRoute: ActivatedRoute,
    private _scanRecSvc: ScanReceiveService,
    private cdr: ChangeDetectorRef,
    private _dll: DropdownlistService,
  ) { }

  public validationForm: FormGroup;
  public user: any;
  public model: ScanReceiveDataView = new ScanReceiveDataView();
  public searchModel: ScanReceiveDataSearchView = new ScanReceiveDataSearchView();

  public data: any = {};
  // public model_scan: ScanSendFinView = new ScanSendFinView();
  // public model_setno : PrintSetNoView = new PrintSetNoView();
  public datas: any = {};
  public datas_print: any = {};
  public data_qty : any;
  public data_docdate : any;
  
  async ngOnInit() {
    this.buildForm();
    this.user = this._authSvc.getLoginUser();
    // this.wclist = await this._dll.getDdlWCSend(this.user.username);
    // if(this.wclist.length==1)
    // {
    //   this.searchModel.wc_code = this.wclist[0].key;
    // }
    //this.searchModel.req_date = new Date()
    //console.log(this.data);
    
  }

  private buildForm() {
    this.validationForm = this._fb.group({
      doc_no: [null, []],
      doc_date: [null, [Validators.required]],
      //send_type:[null, [Validators.required]]
    });
  }
  
  async search(event: PageEvent = null) {   
    if (event != null) {
      this.model.pageIndex = event.pageIndex;
      this.model.itemPerPage = event.pageSize;
    }

    this.data_docdate = this.searchModel.doc_date;
    
     
    var datePipe = new DatePipe("en-US");
    this.searchModel.doc_date = datePipe.transform(this.searchModel.doc_date, 'dd/MM/yyyy').toString();
    this.searchModel.user_id = this.user.username;
    this.searchModel.build_type = this.user.branch.entity_code;

    console.log(this.searchModel);
    this.data =  await this._scanRecSvc.searchDataScanReceive(this.searchModel);
    console.log(this.data);
   
    // this.fin_date.nativeElement.value = this.data_findate;
    this.searchModel.doc_date = this.data_docdate;
   
}


  openScanReceiveNew(p_entity : string ,p_doc_no: string  , _index: number = -1)
  {
    const dialogRef = this._dialog.open(ScanReceiveCreateComponent, {
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

  openProductDetail(p_entity : string ,p_doc_no: string  , _index: number = -1)
  {
    const dialogRef = this._dialog.open(ScanReceiveProductViewComponent, {
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

  radioTypeChange(value) {
    this.searchModel.send_type = value;
    
    console.log(this.searchModel.send_type);
  }


  cancel(_index,scan)
  {

  }

  close() {
    this._router.navigateByUrl('/app/home');
  }


}
