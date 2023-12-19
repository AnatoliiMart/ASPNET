"use strict";
const URL = "https://www.omdbapi.com/?i=tt3896198&apikey=443cb06a&t=";

const btnSearch = document.getElementById("search");
const btnBack = document.getElementById("reset");

btnSearch.onclick = () => {
  getData()
    .then((data) => {
      console.log(data);
      rating.style.display = "block";
      rating.textContent = data.imdbRating;
      filmName.textContent = data.Title;
      year.textContent = "Year: " + data.Year;
      released.textContent = "Released: " + data.Released;
      runtime.textContent = "Runtime: " + data.Runtime;
      genre.textContent = "Genre: " + data.Genre;
      director.textContent = "Director: " + data.Director;
      writer.textContent = "Writer: " + data.Writer;
      actors.textContent = "Actors: " + data.Actors;
      plot.textContent = "Plot: " + data.Plot;
      lang.textContent = "Language: " + data.Language;
      country.textContent = "Country: " + data.Country;
      awards.textContent = "Awards: " + data.Awards;
      main.style.display = "flex";
      reset.style.display = "block";
      searchBlock.style.display = "none";
      document.querySelector("#imgblck>img").setAttribute("src", data.Poster);
    })
    .catch((err) => console.error("Error: ", err));
};
function getData() {
  const inpval = document.getElementById("title");
  return fetch(URL + inpval.value).then((resp) => resp.json());
}
btnBack.onclick = (e) => {
  main.style.display = "none";
  reset.style.display = "none";
  searchBlock.style.display = "block";
};
