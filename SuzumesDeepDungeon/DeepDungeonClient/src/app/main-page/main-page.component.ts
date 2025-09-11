import { Component, OnInit } from '@angular/core';
import { SafeHtml } from '@angular/platform-browser';
import { SafeUrlPipe } from "../Pipes/safe-url.pipe";
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';


@Component({
  selector: 'app-main-page',
  standalone: true,
  imports: [SafeUrlPipe, CommonModule, FormsModule],
  templateUrl: './main-page.component.html',
  styleUrl: './main-page.component.css'
})
export class MainPageComponent implements OnInit {

  iterator: number = 0;

  ngOnInit(): void {
    this.updateStream();
    this.getRandomClip()
    //this.updateClip();
  }

  handleCopyButtonClick(): void {
    this.iterator++;

    switch (true) {
      case this.iterator === 1:
        this.showNotification('Зачем тебе копировать? Не надо так делать, просто читай и наслаждайся.');
        break;
      case this.iterator === 2:
        this.showNotification('Да нет тут реализации копирования по кнопке, просто забей.');
        break;
      case this.iterator === 3:
        this.showNotification('( ͡° ͜ʖ ͡°) ну раз ты так хочешь, тыкай дальше');
        break;
      case this.iterator >= 4 && this.iterator <= 10:
        this.showNotification('(づ ◕‿◕ )づ');
        break;
      case this.iterator > 10 && this.iterator <= 99:
        this.showNotification(`тыкнул ${this.iterator} раз/а`);
        break;
      case this.iterator === 100:
        this.showNotification(`${this.iterator} РАЗ - БЕЗУМИЕ КАКОЕ-ТО! /╲/\╭( ͡°͡° ͜ʖ ͡°͡°)╮/\╱\ обнуляю счетчик...`);
        this.iterator = 0;
        break;
      default:
        this.iterator = 0;
        break;
    }
  }


  private showNotification(message: string): void {

    alert(message);
  }

  channelName: string = 'pan_suzume';
  clipId: string = 'DirtyCrepuscularMallardRlyTho-wCVRyNm8RTBEbCJy';
  streamUrl: string = '';
  clipUrl: string = '';


  updateStream(): void {
    this.streamUrl = `https://player.twitch.tv/?channel=${this.channelName}&parent=localhost&autoplay=false`;
  }

  updateClip(): void {
    this.clipUrl = `https://clips.twitch.tv/embed?clip=${this.clipId}&parent=localhost&autoplay=false`;
  }

  loadStream(): void {
    this.updateStream();
  }

  loadClip(): void {
    this.updateClip();
  }

  clipsId: string[] = ["AssiduousYawningRedpandaHassanChop-ZMK-VX5etf4TxYDO", "DirtyCrepuscularMallardRlyTho-wCVRyNm8RTBEbCJy",
    "RockyBetterKleeDxCat-bBOdomh4LHQtjtTp",
    "CuriousPatientWatercressDeIlluminati-DB_shPsKn1ECR_Dm",
    "StrangeHealthyKleeSMOrc-oyv3lkhKlxVXltwU",
    "EmpathicTentativeMallardDancingBaby-VFrLj9P6sbo3ZNt_",
    "TallWittySpiderChefFrank-jUOkopbCDKC6S4Ke",
    "PunchyExuberantPresidentOSsloth-IX8M03qR0qsZh3-i",
    "CleanIgnorantMeerkatWow-cxaX7aV2HWkx2zfQ",
    "AnimatedBrightPoultryCoolCat-oqZm6Od2QM_gSSCn",
    "DirtyBillowingSkunkBabyRage-LUY3nhG56Mgjt1Vp",
    "SaltyFaintGarlicOneHand-8rOjrzeMhz9o1heX",
    "JollyDeafGoshawkOhMyDog-2dbhpSdsA7CewDaD",
    "PoliteBraveKleeHeyGirl-_xWtcd3U5i21USPN",
    "WanderingShySrirachaCeilingCat-LI_R6NXTuY7iptQx",
    "WrongBenevolentTitanSeemsGood-JaTIXYKPqviflQtB",
    "EntertainingGiantDunlinHeyGirl-64hIvuQxm79B2vKM",
    "EasyLightPorcupinePanicBasket-GuuncP_UA1-oQeCR",
    "DeadCautiousMangoGingerPower--d6Uy-yoTYzjDuuH"

  ]
  getRandomClip() {

    var clipsLength = this.clipsId.length;
    var index = Math.floor(Math.random() * (clipsLength - 0) + 0);

    this.clipId = this.clipsId[index];
    this.loadClip();
  }
}


