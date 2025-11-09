export interface ClientMovie {
    movieId: string;
    movieTitle: string;
    expectedReturnDate: string;
    returnedDate: string | null;
    pricePerDay: number;
}