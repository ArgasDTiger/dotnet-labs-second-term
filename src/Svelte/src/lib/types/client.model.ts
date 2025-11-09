import { ClientMovie } from "./clientMovie.model.ts";
export interface Client {
    id: string;
    firstName: string;
    middleName: string;
    lastName: string;
    phoneNumber: string;
    homeAddress: string;
    passportSeries: string | null;
    passportNumber: string;
    rentedMovies?: ClientMovie[];
}