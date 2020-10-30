import { DatePipe } from '@angular/common';
import { ChangeDetectorRef, Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog, PageEvent } from '@angular/material';
import { Router, ActivatedRoute } from '@angular/router';
import { search } from 'core-js/fn/symbol';
import { scan } from 'rxjs/operators';
import { ScanApproveSendView, ScanApproveSendSearchView } from '../../_model/scan-approve-send';
import { AuthenticationService } from '../../_service/authentication.service';
import { DropdownlistService } from '../../_service/dropdownlist.service';
import { MessageService } from '../../_service/message.service';
import { ScanApproveSendService } from '../../_service/scan-approve-send.service';
import { ScanApproveSendCreateComponent } from '../scan-approve-send-create/scan-approve-send-create.component';
import * as moment from 'moment';

@Component({
  selector: 'app-scan-approve-send-search',
  templateUrl: './scan-approve-send-search.component.html',
  styleUrls: ['./scan-approve-send-search.component.scss']
})
export class ScanApproveSendSearchComponent implements OnInit {

  @ViewChild('fin_date') fin_date: ElementRef;

  constructor(
    private _fb: FormBuilder,
    private _authSvc: AuthenticationService,
    private _dialog: MatDialog,
    private _msgSvc: MessageService,
    private _router: Router,
    private _actRoute: ActivatedRoute,
    private _scanapvSendSvc: ScanApproveSendService,
    private cdr: ChangeDetectorRef,
    private _dll: DropdownlistService,
  ) { }

  public validationForm: FormGroup;
  public user: any;
  public model: ScanApproveSendView = new ScanApproveSendView();
  public searchModel: ScanApproveSendSearchView = new ScanApproveSendSearchView();

  public data: any = {};
  // public model_scan: ScanSendFinView = new ScanSendFinView();
  // public model_setno : PrintSetNoView = new PrintSetNoView();
  public datas: any = {};
  public datas_print: any = {};
  public data_qty : any;
  public data_findate : any;
  
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
      fin_date: [null, [Validators.required]],
      //send_type:[null, [Validators.required]]
    });
  }
  
  async search(event: PageEvent = null) {   

   
    this.model.user_id = this.user.username;
    this.data_findate = this.searchModel.fin_date;
    
    // this.dataPending.dataTotals  = [];
    // this.dataForward.dataTotals  = [];

    // sessionStorage.setItem('sendApv-finDate', this.fin_date.nativeElement.value);
    // sessionStorage.setItem('jobOperation-docNo', this.validationForm.get('doc_no').value);
    // sessionStorage.setItem('jobOperation-build_type', this.validationForm.get('build_type').value);
 
    var datePipe = new DatePipe("en-US");
    this.searchModel.fin_date = datePipe.transform(this.searchModel.fin_date, 'dd/MM/yyyy').toString();
    this.searchModel.user_id = this.user.username;
    this.searchModel.build_type = this.user.branch.entity_code;

    console.log(this.searchModel);
    this.data =  await this._scanapvSendSvc.searchScanApproveSend(this.searchModel);
    console.log(this.data);
   
    // this.fin_date.nativeElement.value = this.data_findate;
    this.searchModel.fin_date = this.data_findate;
   
}


  openScanApproveNew(p_entity : string ,p_fin_date: string, _index: number = -1)
  {
    const dialogRef = this._dialog.open(ScanApproveSendCreateComponent, {
      maxWidth: '100vw',
      maxHeight: '100vh',
      height: '100%',
      width: '100%',
      data: {
        fin_date: p_fin_date,
        entity:p_entity
      }

    });

    dialogRef.afterClosed().subscribe(result => {
      if (result.length > 0) {
        //this.add_prod(result);
      }
    })
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
