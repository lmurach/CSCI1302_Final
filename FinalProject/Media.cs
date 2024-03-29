﻿using System;
using System.Collections.Generic;

public class Media {

    const int TYPE = 0, TITLE = 1, COPYRIGHT_YEAR = 2;
    public const string BOOK = "1", MAGAZINE = "2", MOVIE = "3"; 
    public string type { get => _type; set => _type = value; }
    private string _type;
    public string title { get => _title; set => _title = value; }
    private string _title;
    public int copyrightYear { get => _copyrightYear; set => _copyrightYear = value; }
    private int _copyrightYear;
    public int lineNumber { get => _lineNumber; set => _lineNumber = value; }
    private int _lineNumber;

    public static List<Media> mediaStorage = new List<Media>();

    public virtual void parse_array_to_var(string[] parseString) {

        void parse_title(string[] parseStringInside) {
            try {
                title = parseStringInside[TITLE];
            }
            catch {
                ErrorHandling.error_create_and_add_list(
                        type, lineNumber, String.Join(",", parseStringInside), $"Type is invalid. " +
                        $"Must be either a book ({Media.BOOK}), movie ({Media.MOVIE}), " +
                        $"or magazine ({Media.MAGAZINE}).", this);
            }
        }
        void parse_copyright_year(string[] parseStringInside) {
            try {
                const int MIN_COPYRIGHT_YEAR = 1790; 
                // apparently this is when the US Constitution gave power for copyright..
                // the more you know.
                copyrightYear = Int32.Parse(parseStringInside[COPYRIGHT_YEAR]);
                if (copyrightYear > DateTime.Today.Year || copyrightYear < MIN_COPYRIGHT_YEAR) {
                    ErrorHandling.error_create_and_add_list(
                            type, lineNumber, String.Join(",", parseStringInside),
                            "Copyright year is out of bounds.", this);
                }
            }
            catch {
                ErrorHandling.error_create_and_add_list(
                    type, lineNumber, String.Join(",", parseStringInside),
                    "Copyright year field is invalid.", this);
            }
        }
        parse_title(parseString);
        parse_copyright_year(parseString);
    }

    public static int number_of_records() {
        return mediaStorage.Count;
    }

    public static int oldest_copyright_year() {
        int oldest = Int32.MaxValue;
        foreach (Media record in mediaStorage) {
            if (record.copyrightYear < oldest) {
                oldest = record.copyrightYear;
            }
        }
        return oldest;
    }

    public static int newest_copyright_year() {
        int youngest = 0;
        foreach (Media record in mediaStorage) {
            if (record.copyrightYear > youngest) {
                youngest = record.copyrightYear;
            }
        }
        return youngest;
    }
    public static int median_copyright_year() {
        List<int> tempMedia = new List<int>();
        foreach (Media media in mediaStorage) {
            tempMedia.Add(media.copyrightYear);
        }
        tempMedia.Sort();
        int length = tempMedia.Count;
        // even median (average of two middle)
        if (length % 2 == 0) {
            int medianFirst = tempMedia[length / 2];
            int medianSecond = tempMedia[(length / 2) + 1];
            return (medianFirst + medianSecond) / 2;
        }
        else {
            return tempMedia[(length / 2)];
        }
    }
    public static int total_pages() {
        // friend of mine told me about this "is" keyword and helped me out here.
        int pages = 0;
        foreach (Media media in mediaStorage) {
            if (media is Book book) {
                pages += book.numberOfPages;
            }
        }
        return pages;
    }
}

class Book : Media {
    const int NUMBER_OF_PAGES = 3, AUTHOR = 4;
    public int numberOfPages;
    string author;

    public Book() {
        type = "book";
    }
    public override void parse_array_to_var(string[] input) {
        base.parse_array_to_var(input);
        void parse_number_of_pages(string[] inputInside) {
            try {
                numberOfPages = Int32.Parse(inputInside[NUMBER_OF_PAGES]);
            }
            catch {
                ErrorHandling.error_create_and_add_list("number of pages", lineNumber, String.Join(",", inputInside), 
                    "Number of pages field is invalid.", this);
            }
        }
        void parse_author(string[] inputInside) {
            try {
                author = input[AUTHOR];
            }
            catch {
                ErrorHandling.error_create_and_add_list("author", lineNumber, String.Join(",", inputInside), 
                    "Author field is invalid.", this);
            }
        }
        parse_number_of_pages(input);
        parse_author(input);

    }


}

class Magazine : Media {
    const int EDITOR = 3;
    string editor;
    
    public Magazine() {
        type = "magazine";
    }
    public override void parse_array_to_var(string[] input) {
        base.parse_array_to_var(input);
        void parse_editor(string[] inputInside) {
            try {
                editor = input[EDITOR];
            }
            catch {
                ErrorHandling.error_create_and_add_list("editor", lineNumber, String.Join(",", inputInside), 
                    "Editor field is invalid.", this);
            }
        }
        parse_editor(input);
    }


}

class Movie : Media {
    const int LENGTH_IN_MINUTES = 3, RELEASE_DATE = 4;
    int lengthInMinutes;
    DateTime releaseDate;

    public Movie() {
        type = "movie";
    }
    public override void parse_array_to_var(string[] input) {
        base.parse_array_to_var(input);
        void parse_length_in_minutes(string[] inputInside) {
            try {
                lengthInMinutes = Int32.Parse(input[LENGTH_IN_MINUTES]);
            }
            catch {
                ErrorHandling.error_create_and_add_list("length in minutes", lineNumber, String.Join(",", inputInside), 
                    "Length in minutes field is invalid.", this);
            }
        }
        void parse_release_date(string[] inputInside) {
            try {
                releaseDate = DateTime.Parse(input[RELEASE_DATE]);
            }
            catch {
                ErrorHandling.error_create_and_add_list("release date", lineNumber, String.Join(",", inputInside), 
                    "Release date is invalid.", this);
            }
        }
        parse_length_in_minutes(input);
        parse_release_date(input);       
    }
}
