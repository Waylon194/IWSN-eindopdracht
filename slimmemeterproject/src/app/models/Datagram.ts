import { Telegram } from '../models/Telegram';
import { SObject } from '../models/SObject';

export interface Datagram {
    telegram: Telegram,
    signature: string,
    carCharger: SObject,
    solarPanel: SObject,
}