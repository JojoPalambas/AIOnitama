using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICoucFirst : AI
{
    private Team MaTeam;
    private Team SaTeam;
    private Card MaCarte1;
    private Card MaCarte2;
    private Card SaCarte1;
    private Card SaCarte2;

    public override void Init(Team team)
    {
        this.MaTeam = team;
        if (MaTeam == Team.A)
            {
            SaTeam = Team.B;
            MaCarte1 = InfoGiver.cardA1;
            MaCarte2 = InfoGiver.cardA2;
            SaCarte1 = InfoGiver.cardB1;
            SaCarte2 = InfoGiver.cardB2;
            }
        else
            {
            SaTeam = Team.A;
            MaCarte1 = InfoGiver.cardB1;
            MaCarte2 = InfoGiver.cardB2;
            SaCarte1 = InfoGiver.cardA1;
            SaCarte2 = InfoGiver.cardA2;
        }
        
        return;
    }

    // Makes the list of all the possible turns, then picks up a random one
    public override TurnResponse PlayTurn()
    {
        PieceState[][] PlateauCourant = InfoGiver.table;
        List<TurnResponse> MesCoupsPossibles = ComputeCoupsPossibles(PlateauCourant, MaTeam);
        List<TurnResponse> SesCoupsPossibles = ComputeCoupsPossibles(PlateauCourant, SaTeam);

        TurnResponse MeilleurCoup = EvalFinale(PlateauCourant,MaTeam,MesCoupsPossibles,SesCoupsPossibles);

        return MeilleurCoup;

        
    }

    private List<TurnResponse> ComputeCoupsPossibles(PieceState[][] Plateau, Team team)
    {
        List<TurnResponse> ListeDeCoupsPossibles = new List<TurnResponse>();
        Card Carte1;
        Card Carte2;
        if (team == Team.A) { Carte1 = InfoGiver.cardA1; Carte2 = InfoGiver.cardA2; }
        else { Carte1 = InfoGiver.cardB1; Carte2 = InfoGiver.cardB2; }

       for (int i=0; i<=4; i++)
        {
            for (int j = 0; j <= 4; j++)
            {
                 if (Plateau[i][j] != null)
                    {
                    if (Plateau[i][j].team == team)
                        {
                            int[][] moves;
                            for (int IndiceCarte = 1; IndiceCarte <= 2; IndiceCarte++)
                            {
                                Card Carte;
                                if (IndiceCarte == 1) Carte = Carte1; else Carte = Carte2;
                                if (team == Team.A) moves = Carte.GetMoves(); else moves = Carte.GetMovesReversed();
                                for (int X=0; X<=4; X++)
                                    {
                                    for (int Y=0; Y<=4; Y++)
                                        {
                                        if (moves[X][Y]==1)
                                            {
                                            int ii = i + X - 2;
                                            int jj = j + Y - 2;
                                            if (ii>=0 && ii<=4 && jj>=0 && jj<=4)    // reste dans le damier
                                                {
                                                bool CoupCorrect = true;
                                                if (Plateau[ii][jj]!=null)                // il y a déjà une pièce
                                                    {
                                                    if (Plateau[ii][jj].team == team) CoupCorrect = false; // pièce du même camp
                                                    }
                                                if (CoupCorrect==true)
                                                    {
                                                    ListeDeCoupsPossibles.Add(new TurnResponse(Carte.cardName,
                                                                                                new Vector2Int(i,j),
                                                                                                new Vector2Int(i+X-2,j+Y-2)));
                                                    }
                                                }
                                            }
                                        }
                                    }
                            }
                        }
                    }

            }
       }

        if (ListeDeCoupsPossibles.Count == 0) return null;
        else return ListeDeCoupsPossibles;
    }

    private TurnResponse EvalFinale(PieceState[][] Plateau, Team MaTeam, List<TurnResponse> MesCoups, List<TurnResponse> SesCoups)
        {
        double CaseSafe = 0.9;
        double CaseMenacee = 1.0;
        double BiaisDuRoi = 0.9;

        double CoupGagnant = 2.0;
        double PriseMonk = 0.9;
        double PasDePrise = 0.8;
        double DestMenacee = 0.1;

        double CaseCouverte = 1.0;  // si la case est couverte, on ne diminbue pas l'Eval
        double CaseCouvrante = 1.0; // idem si on se met en position de couvrir un allié
        double CaseIsolee = 0.75;   // ni l'un ni l'autre

        List<double> Eval = new List<double>();

        for (int i = 0; i < MesCoups.Count; i++) Eval.Add(1.0);

        for (int i=0; i<MesCoups.Count;i++)
        {
            int X = MesCoups[i].source.x;
            int Y = MesCoups[i].source.y;
            int dX = MesCoups[i].destination.x;
            int dY = MesCoups[i].destination.y;

            // le roi sur le chateau adverse
            if (Plateau[X][Y].type==PieceType.king)
                {
                Eval[i] = Eval[i] * BiaisDuRoi; // de toute façon, on applique le biais pour diminuer la proba de bouger le roi
                if (MaTeam == Team.A && dX == 2 && dY == 4) Eval[i] = CoupGagnant;
                if (MaTeam == Team.B && dX == 2 && dY == 0) Eval[i] = CoupGagnant;
                }

            // Case Menacée ?
            if (Eval[i] < CoupGagnant)
            {
                for (int j = 0; j < SesCoups.Count; j++)
                {
                    int ddX = SesCoups[j].destination.x;
                    int ddY = SesCoups[j].destination.y;

                    if (X == ddX && Y == ddY) { Eval[i] = Eval[i] * CaseMenacee; /*Debug.Log("===== DEST MENACEE =======");*/ }
                    else Eval[i] = Eval[i] * CaseSafe;
                }

                // Prises
                if (Plateau[dX][dY] == null)
                {
                    Eval[i] = Eval[i] * PasDePrise;
                }
                else
                {
                    if (Plateau[dX][dY].type == PieceType.king) Eval[i] = CoupGagnant;
                    if (Plateau[dX][dY].type == PieceType.monk) Eval[i] = Eval[i] * PriseMonk;
                }

                // case menacées
                for (int j = 0; j < SesCoups.Count; j++)
                {
                    int ddX = SesCoups[j].destination.x;
                    int ddY = SesCoups[j].destination.y;
                    if (ddX == dX && ddY == dY) // la destination est menacée par une pièce adverse
                    {
                        if (Eval[i] < CoupGagnant) { Eval[i] = Eval[i] * DestMenacee; /*Debug.Log("===== DEST MENACEE =======");*/ }
                    }
                }
            }

            // couvertures
            // compliqué car demande de prévoir les cartes suivantes et de tout recalculer avec...

           /* Debug.Log("i=" + i.ToString()
                        + "   (" + X.ToString()+","+ Y.ToString()+") => ("
                        + dX.ToString()+","+dY.ToString()+")"
                        + " - Eval =" + Eval[i].ToString());*/
        }

        //Debug.Log("taille du tableau Eval = " + Eval.Count.ToString());
        
        // Tire au sort parmi les EvalMax
        double EvalMax = Eval[0];
        for (int i=1; i<Eval.Count; i++)
            {
            if (Eval[i] > EvalMax) EvalMax = Eval[i];
            }
       // Debug.Log("EvalMax = " + EvalMax);

        List<int> IndicesPossibles = new List<int>();
        for (int i=0; i<Eval.Count;i++)
            {
            if (Eval[i] >= EvalMax)
                {
                IndicesPossibles.Add(i);
                //Debug.Log("indice possible = " + i.ToString());
                }
            }

        int IndiceChoisi = 0;
        if (IndicesPossibles.Count>0)
            {
            IndiceChoisi = Random.Range(0, IndicesPossibles.Count);
            }
        //Debug.Log("Indice choisi = " + IndicesPossibles[IndiceChoisi].ToString());
        return MesCoups[IndicesPossibles[IndiceChoisi]];
        }



    public override string name
    {
        get { return "AI Couc First"; }
    }
}
