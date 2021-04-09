import { Datagram } from './Datagram'

export interface SmartMeterMeasurement{
    id: number,
    dateOfMeasurement: Date,
    datagram: Datagram
}