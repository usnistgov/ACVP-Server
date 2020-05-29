import { Context, Recogniser } from '.';
export default class Utf8 implements Recogniser {
    name(): string;
    match(det: Context): any;
}
