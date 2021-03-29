import { Datagram } from '../models/Datagram'

export interface SmartMeterMeasurement{
    id: number,
    dateOfMeasurement: Date,
    datagram: Datagram
}