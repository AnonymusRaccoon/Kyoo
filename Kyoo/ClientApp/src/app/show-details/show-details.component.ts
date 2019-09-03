import { HttpClient } from "@angular/common/http";
import { Component, OnInit } from '@angular/core';
import { MatSnackBar } from "@angular/material/snack-bar";
import { DomSanitizer, Title } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';
import { Episode } from "../../models/episode";
import { Show } from "../../models/show";

@Component({
  selector: 'app-show-details',
  templateUrl: './show-details.component.html',
  styleUrls: ['./show-details.component.scss']
})
export class ShowDetailsComponent implements OnInit
{
  show: Show;
  episodes: Episode[] = null;
  season: number;

  private toolbar: HTMLElement;
  private backdrop: HTMLElement;
  private peopleScroll: HTMLElement;

  constructor(private route: ActivatedRoute, private sanitizer: DomSanitizer, private http: HttpClient, private snackBar: MatSnackBar, private title: Title)
  {
    this.route.queryParams.subscribe(params =>
    {
      this.season = params["season"];
    });
  }

  ngOnInit()
  {
    this.show = this.route.snapshot.data.show;
    this.title.setTitle(this.show.title + " - Kyoo");

    if (this.season == null || this.show.seasons.find(x => x.seasonNumber == this.season) == null)
      this.season = 1;

    this.toolbar = document.getElementById("toolbar");
    this.backdrop = document.getElementById("backdrop");
    this.peopleScroll = document.getElementById("peopleScroll");
    window.addEventListener("scroll", this.scroll, true);
    this.toolbar.setAttribute("style", `background-color: rgba(0, 0, 0, 0) !important`);

    this.getEpisodes();
  }

  ngAfterViewInit()
  {
    $('[data-toggle="tooltip"]').tooltip();
  }

  ngOnDestroy()
  {
    window.removeEventListener("scroll", this.scroll, true);
    this.title.setTitle("Kyoo");
    this.toolbar.setAttribute("style", `background-color: #000000 !important`);
  }

  scroll = () =>
  {
    let opacity: number = 2 * window.scrollY / this.backdrop.clientHeight;
    this.toolbar.setAttribute("style", `background-color: rgba(0, 0, 0, ${opacity}) !important`);
  }

  getEpisodes()
  {
    if (this.show == null)
      return;

    if (this.show.seasons.find(x => x.seasonNumber == this.season).episodes != null)
      this.episodes = this.show.seasons.find(x => x.seasonNumber == this.season).episodes;


    this.http.get<Episode[]>("api/episodes/" + this.show.slug + "/season/" + this.season).subscribe((episodes: Episode[]) =>
    {
      this.show.seasons.find(x => x.seasonNumber == this.season).episodes = episodes;
      this.episodes = episodes;
    }, error =>
    {
      console.log(error.status + " - " + error.message);
      this.snackBar.open("An unknow error occured while getting episodes.", null, { horizontalPosition: "left", panelClass: ['snackError'], duration: 2500 });
    });
  }


  scrollLeft()
  {
    let scroll: number = this.peopleScroll.offsetWidth * 0.80;
    this.peopleScroll.scrollBy({ top: 0, left: -scroll, behavior: "smooth" });

    document.getElementById("pl-rightBtn").classList.remove("d-none");

    if (this.peopleScroll.scrollLeft - scroll <= 0)
      document.getElementById("pl-leftBtn").classList.add("d-none");
  }

  scrollRight()
  {
    let scroll: number = this.peopleScroll.offsetWidth * 0.80;
    console.log("Scroll: " + scroll);
    this.peopleScroll.scrollBy({ top: 0, left: scroll, behavior: "smooth" });
    document.getElementById("pl-leftBtn").classList.remove("d-none");

    if (this.peopleScroll.scrollLeft + scroll >= this.peopleScroll.scrollWidth - this.peopleScroll.clientWidth)
      document.getElementById("pl-rightBtn").classList.add("d-none");
  }

  getPeopleIcon(slug: string)
  {
    return this.sanitizer.bypassSecurityTrustStyle("url(/peopleimg/" + slug + ")");
  }
}
