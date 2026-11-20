import '../core/arcade.dart';
import '../core/missions.dart';
import '../core/stats.dart';
import 'civ_spec.dart';
import 'tycoon_rules.dart';

class CivRules extends TycoonRules {
  CivRules()
      : super(civSpec, 'game-022-mini-civilization', const [
          GameStat('population_peak', 'En yüksek nüfus', 'Peak population', scale: 50),
          GameStat('buildings_bought', 'Kurulan bina', 'Buildings built', scale: 40),
          GameStat('money_earned', 'Kazanılan altın', 'Gold earned', scale: 5000),
        ], const [
          MissionTemplate('population_peak', 'population_peak', [16, 33, 50]),
          MissionTemplate('buildings_bought', 'buildings_bought', [13, 26, 40]),
        ]);
}

ArcadeRules createRules() => CivRules();
